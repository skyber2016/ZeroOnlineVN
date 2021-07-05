using Newtonsoft.Json;

namespace CORE_API.Middlewares
{
    public class CryptoRequest
    {
        /// <summary>
        /// Là mã hóa của AES, d = IV + AES(body)
        /// </summary>
        [JsonProperty("d")]
        public string Data { get; set; }
        /// <summary>
        /// là mã hóa của thuật toán RSA, bên trong là AESKey, k = RSA(AESKey)
        /// </summary>
        [JsonProperty("k")]
        public string Key { get; set; }
    }

    public class CryptoPublicRequest
    {
        public string ClientPubKey { get; set; }
    }
}
