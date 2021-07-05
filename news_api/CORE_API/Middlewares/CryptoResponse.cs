using Newtonsoft.Json;

namespace CORE_API.Middlewares
{
    public class CryptoResponse
    {
        /// <summary>
        /// Data
        /// </summary>
        [JsonProperty("d")]
        public string Data { get; set; }
        /// <summary>
        /// Data
        /// </summary>
        [JsonProperty("k")]
        public string Key { get; set; }
    }
}
