using CORE_API.Middlewares;
using NETCore.Encrypt;
using NETCore.Encrypt.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CORE_API.Helpers
{
    public static class CryptoHelper
    {
        /// <summary>
        /// Step 1: Tạo public key từ private key lưu trong appsetting
        /// Step 2: Tạo key aes-256
        /// Step 3: Mã hoá: Response { k: string, d: string }
        /// Mã hóa data = AES256 với (aesKey, IV), sau đó thì d = IV + data
        /// Mã hóa k = RSA(aesKey)
        /// Step 4: Giải mã: làm ngược lại từ k để lấy aeskey + 16 byte đầu của d (IV), dùng AES(key,IV) để giải mã d từ byte thứ 16 trở đi, vì 16 byte đầu là IV
        /// ----
        /// </summary>
        public static CryptoResponse Encrypt(string publicKeyBase64, string body)
        {
            try
            {
                var aesKey = Aes.Create();
                // chuỗi final = iv + enc data
                var data = AESEncrypt(Encoding.UTF8.GetBytes(body), aesKey.Key, aesKey.IV).ToList();
                var dataTemp = new List<byte>();
                dataTemp.AddRange(aesKey.IV);
                dataTemp.AddRange(data);
                var key = RSAEncrypt(publicKeyBase64, aesKey.Key);
                return new CryptoResponse
                {
                    Data = Convert.ToBase64String(dataTemp.ToArray()),
                    Key = key
                };
            }
            catch (Exception ex)
            {
                throw new MethodAccessException(ex.Message, ex);
            }
            
        }

        public static string Decrypt(string privateKeyBase64, CryptoRequest request)
        {
            try
            {
                var dataDecode = Convert.FromBase64String(request.Data);
                var iv = dataDecode.Take(16).ToArray();
                var dataEnc = dataDecode.Skip(16).Take(dataDecode.Length).ToArray();
                var privateKey = Encoding.UTF8.GetString(Convert.FromBase64String(privateKeyBase64));
                var aesKeyBase64 = RSADecrypt(privateKey, request.Key);
                var aesKey = Convert.FromBase64String(aesKeyBase64);
                var dataDec = AESDecrypt(dataEnc, aesKey, iv);
                return dataDec;
            }
            catch (Exception ex)
            {
                throw new MethodAccessException(ex.Message, ex);
            }
            
        }

        public static string RSAEncrypt(string publicKeyPem, byte[] src)
        {
            var encrypted = EncryptProvider.RSAEncrypt(publicKeyPem, src, RSAEncryptionPadding.Pkcs1, true);
            return Convert.ToBase64String(encrypted);
        }

        public static string RSAEncryptFromPublicKeyBase64(string publicKeyFromBase64, byte[] src)
        {
            var pub = Encoding.UTF8.GetString(Convert.FromBase64String(publicKeyFromBase64));
            return RSAEncrypt(pub, src);
        }

        public static string RSADecrypt(string privateKeyPem, string base64src)
        {
            var decrypted = EncryptProvider.RSADecrypt(privateKeyPem, Convert.FromBase64String(base64src), RSAEncryptionPadding.Pkcs1, true);
            return Encoding.UTF8.GetString(decrypted);
        }
        /// <summary>
        /// AES encrypt
        /// </summary>
        /// <param name="data">Raw data</param>  
        /// <param name="key">Key, requires 32 bits</param>  
        /// <param name="vector">IV,requires 16 bits</param>  
        /// <returns>Encrypted byte array</returns>  
        public static byte[] AESEncrypt(byte[] data, byte[] key, byte[] vector)
        {
            Check.Argument.IsNotEmpty(data, nameof(data));

            Check.Argument.IsNotEmpty(key, nameof(key));
            Check.Argument.IsNotOutOfRange(key.Length, 32, 32, nameof(key));

            Check.Argument.IsNotEmpty(vector, nameof(vector));
            Check.Argument.IsNotOutOfRange(vector.Length, 16, 16, nameof(vector));

            byte[] plainBytes = data;
            byte[] bKey = key;
            byte[] bVector = vector;

            byte[] encryptData = null; // encrypted data
            using (Aes Aes = Aes.Create())
            {
                try
                {
                    using (MemoryStream Memory = new MemoryStream())
                    {
                        Aes.Mode = CipherMode.CBC;
                        var ags = Aes.CreateEncryptor(bKey, bVector);
                        using (CryptoStream Encryptor = new CryptoStream(Memory, ags,CryptoStreamMode.Write))
                        {
                            Encryptor.Write(plainBytes, 0, plainBytes.Length);
                            Encryptor.FlushFinalBlock();

                            encryptData = Memory.ToArray();
                        }
                    }
                }
                catch
                {
                    encryptData = null;
                }
                return encryptData;
            }
        }
        public static string AESDecrypt(byte[] data, byte[] key, byte[] vector)
        {
            Check.Argument.IsNotEmpty(data, nameof(data));

            Check.Argument.IsNotEmpty(key, nameof(key));
            Check.Argument.IsNotOutOfRange(key.Length, 32, 32, nameof(key));

            Check.Argument.IsNotEmpty(vector, nameof(vector));
            Check.Argument.IsNotOutOfRange(vector.Length, 16, 16, nameof(vector));

            byte[] encryptedBytes = data;
            byte[] bKey = key;
            byte[] bVector = vector;

            byte[] decryptedData = null; // decrypted data

            using (Aes Aes = Aes.Create())
            {
                using (MemoryStream Memory = new MemoryStream(encryptedBytes))
                {
                    Aes.Mode = CipherMode.CBC;
                    var arg = Aes.CreateDecryptor(bKey, bVector);
                    using (CryptoStream Decryptor = new CryptoStream(Memory, arg, CryptoStreamMode.Read))
                    {
                        using (MemoryStream tempMemory = new MemoryStream())
                        {
                            byte[] Buffer = new byte[1024];
                            Int32 readBytes = 0;
                            while ((readBytes = Decryptor.Read(Buffer, 0, Buffer.Length)) > 0)
                            {
                                tempMemory.Write(Buffer, 0, readBytes);
                            }

                            decryptedData = tempMemory.ToArray();
                        }
                    }
                }
                if (decryptedData == null)
                {
                    return null;
                }
                return Encoding.UTF8.GetString(decryptedData);
            }
        }
    }
}
