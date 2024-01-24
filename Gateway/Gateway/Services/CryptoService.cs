using Gateway.Models;
using NETCore.Encrypt;
using NETCore.Encrypt.Shared;
using Newtonsoft.Json;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Gateway.Services
{
    public static class CryptoService
    {
        public static BaseRequest Decrypt(GatewayModel request)
        {
            if (request == null)
            {
                return default;
            }

            var basePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            var privatePEM_Path = Path.Combine(basePath, "private.PEM");

            var privatePEM = File.ReadAllText(privatePEM_Path);

            var keyBytes = Convert.FromBase64String(request.Key);
            var dataBytes = Convert.FromBase64String(request.Data);
            var rsa = EncryptProvider.RSAFromPem(privatePEM);
            var aesKeyStr = Encoding.UTF8.GetString(rsa.Decrypt(keyBytes, RSAEncryptionPadding.Pkcs1));
            var vector = Encoding.UTF8.GetString(dataBytes.Take(16).ToArray());
            var datas = dataBytes.Skip(16).ToArray();
            var dataDecryptBytes = EncryptProvider.AESDecrypt(datas, aesKeyStr, vector);
            var dataJson = Encoding.UTF8.GetString(dataDecryptBytes);
            var baseData = JsonConvert.DeserializeObject<BaseRequest>(dataJson);
            return baseData;

        }
        public static GatewayModel Encrypt(BaseRequest request, object dataResponse)
        {
            if (request == null)
            {
                return default;
            }
            var jsonString = JsonConvert.SerializeObject(dataResponse);
            var dataBytes = Encoding.UTF8.GetBytes(jsonString);

            var aes = EncryptProvider.CreateAesKey();
            var aesKeyBytes = Encoding.UTF8.GetBytes(aes.Key);
            var datasResponse = new List<byte>();
            datasResponse.AddRange(Encoding.UTF8.GetBytes(aes.IV));
            var dataEnc = EncryptProvider.AESEncrypt(dataBytes, aes.Key, aes.IV);
            datasResponse.AddRange(dataEnc);
            var keyBytes = EncryptProvider.RSAEncryptWithPem(request.ClientPublicKey, aesKeyBytes);
            var response = new GatewayModel
            {
                Key = Convert.ToBase64String(keyBytes),
                Data = Convert.ToBase64String(datasResponse.ToArray())
            };
            return response;
        }

    }
}
