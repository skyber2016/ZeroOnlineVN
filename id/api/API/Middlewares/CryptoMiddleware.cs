using API.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace API.Middlewares
{
    public class CryptoMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly CryptoSettings CryptoSettings;
        private readonly ILoggerManager Logger;
        private readonly IWebHostEnvironment Env;
        public CryptoMiddleware(RequestDelegate next, IOptions<CryptoSettings> options, ILoggerManager logger, IWebHostEnvironment env)
        {
            this._next = next;
            this.CryptoSettings = options.Value;
            this.Logger = logger;
            this.Env = env;
        }
        /// <summary>
        /// Sau khi server nhận được request có dạng { k: string, d: string }
        /// Giải mã request -> Controller logic -> mã hóa response
        /// Thuật toán:
        ///     AES: Sử dụng AES Key và IV để mã hóa
        ///     RSA: Sử dụng public key để mã hóa và private key để giải mã
        /// Cấu trúc mã hóa:
        ///     {
        ///         k: Bên trong là AES Key; được mã hóa bằng public key của server -> sử dụng private key server để giải mã -> nhận được AES Key
        ///         d: Bên trong có { ClientPubKey,... } , được mã hóa bằng AES
        ///             d = Base64 (IV(16byte) + dataEncrypt(AES))
        ///         => Từ 'k' ta được AES Key và từ 'd' ta được IV
        ///         => Sử dụng AES Key + IV để giải mã dataEncrypt(AES) -> json request
        ///     }
        ///  Các bước mã hóa:
        ///     Step 1: Ta có jsonResponse = { username: 'abc', password: 'xyz' }
        ///     Step 2: Tạo ra ngẫu nhiên 2 key: AES Key(32byte) và IV(16byte)
        ///     Step 3: Sử dụngAES(jsonResponse) để mã hóa jsonResponse -> jsonResponse = [24,21,52,32,...] byte array
        ///     Step 4: Tạo ra d = Base64(IV(16byte) + jsonResponse(nByte)) -> jsonResponse = "qcvdgrhmh...==";
        ///     Step 5: Tạo ra k = RSA(AES Key(32byte)) => sử dụng ClientPubKey để mã hóa, ClientPubKey lấy ở bước giải mã request
        ///     Step 6: Trả về object bao gồm { k: string, d: string } cho client
        ///  Các bước giải mã:
        ///     Step 1: Dùng private Key được lưu sẵn trong appsetting để giải mã 'k' -> ta được AES Key (32byte)
        ///     Step 2: Convert 'd' từ Base64 về byte[] -> gọi là dataDecoded
        ///     Step 3: Lấy 16 byte đầu tiên từ dataDecoded ra làm IV(16byte)
        ///     Step 4: Lấy phần còn lại của dataDecoded làm dataEncrypted
        ///     Step 5: Sử dụng AES Key(32Byte) + IV(16byte) từ 1 và 3 để giải mã dataEncrypted; ta được jsonRequest = AESDecrypt(dataEncrypted)
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            if (this.IsSwagger(httpContext) || !httpContext.Request.Path.StartsWithSegments("/api"))
            {
                await this._next(httpContext);
                return;
            }
            var request = httpContext.Request.Body;
            var response = httpContext.Response.Body;
            try
            {
                httpContext.Request.EnableBuffering();

                using (var newRequest = new MemoryStream())
                {
                    using (var newResponse = new MemoryStream())
                    {
                        httpContext.Response.Body = newResponse;
                        string body = string.Empty;
                        var header = httpContext.Request.Headers;
                       if(httpContext.Request.Method.ToUpper() == "GET" || (header.ContainsKey("d") && header.ContainsKey("k")))
                        {
                            var data = httpContext.Request.Headers["d"];
                            var key = httpContext.Request.Headers["k"];
                            if(!string.IsNullOrEmpty(data) && !string.IsNullOrEmpty(key))
                            {
                                body = JsonConvert.SerializeObject(new CryptoRequest
                                {
                                    Data = data,
                                    Key = key
                                });
                            }
                            else
                            {
                                throw new MethodAccessException("Not found key and data in headers");
                            }
                        }
                        else
                        {
                            using (var reader = new StreamReader(httpContext.Request.Body))
                            {
                                // read the contents of the original request stream
                                body = await reader.ReadToEndAsync();
                            }
                        }
                        
                        string clientPublicKey= "";
                        if (string.IsNullOrEmpty(body))
                        {
                            await _next(httpContext);
                        }
                        else
                        {
                            httpContext.Request.Body = newRequest;
                            var modelRequest = JsonConvert.DeserializeObject<CryptoRequest>(body);
                            if(modelRequest.Data == null || modelRequest.Key == null)
                            {
                                throw new MethodAccessException($"Cannot convert body to CryptoRequest, body: {body}");
                            }
                            var privateKey = this.CryptoSettings.PrivateKey;
                            var jsonRequest = CryptoHelper.Decrypt(privateKey, modelRequest);

                            var publicKeyData = JsonConvert.DeserializeObject<CryptoPublicRequest>(jsonRequest);
                            clientPublicKey = publicKeyData.ClientPubKey;
                            using (var writer = new StreamWriter(newRequest))
                            {
                                await writer.WriteAsync(jsonRequest);
                                await writer.FlushAsync();
                                newRequest.Position = 0;
                                httpContext.Request.Body = newRequest;
                                await _next(httpContext);
                            }
                        }
                        using (var reader = new StreamReader(newResponse))
                        {
                            newResponse.Position = 0;
                            var responseStr = await reader.ReadToEndAsync();
                            if (!string.IsNullOrEmpty(responseStr))
                            {
                                if (string.IsNullOrEmpty(clientPublicKey))
                                {
                                    throw new ArgumentException("Not found client public key");
                                }
                                var modelResponse = CryptoHelper.Encrypt(clientPublicKey, responseStr);
                                using (var w = new StreamWriter(response))
                                {
                                    var jsonResponse = JsonConvert.SerializeObject(modelResponse);
                                    await w.WriteAsync(jsonResponse);
                                    await w.FlushAsync();
                                }
                            }
                        }
                    }

                }
            }
            catch(MethodAccessException ex)
            {
                this.Logger.Error(ex.Message);
                httpContext.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex.Message);
                this.Logger.Error(ex.StackTrace);
                using (var w = new StreamWriter(response))
                {
                    var message = "Hệ thống hiện tại đang lỗi, vui lòng thực hiện lại sau";
                    var jsonResponse = new
                    {
                        message = message
                    };
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await w.WriteAsync(JsonConvert.SerializeObject(jsonResponse));
                    await w.FlushAsync();
                }
            }
            finally
            {
                httpContext.Request.Body = request;
                httpContext.Response.Body = response;
            }
            httpContext.Response.OnCompleted(() =>
            {
                return Task.CompletedTask;
            });
        }
        private bool IsSwagger(HttpContext context)
        {
            var isDev = this.Env.IsDevelopment();
            var referrer = context.Request.Headers["Referer"].ToString();
            return isDev && referrer == "http://localhost:63266/index.html";
        }
    }
}
