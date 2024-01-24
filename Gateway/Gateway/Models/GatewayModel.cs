using System.Text.Json.Serialization;

namespace Gateway.Models
{
    public class GatewayModel
    {
        /// <summary>
        /// Là mã hóa của AES, d = IV + AES(body)
        /// </summary>
        [JsonPropertyName("d")]
        public string Data { get; set; }
        /// <summary>
        /// là mã hóa của thuật toán RSA, bên trong là AESKey, k = RSA(AESKey)
        /// </summary>
        [JsonPropertyName("k")]
        public string Key { get; set; }

    }

    public class BaseRequest
    {
        public string ClientPublicKey { get; set; }
        public string Mid {  get; set; }
        public object Request {  get; set; }
    }

}
