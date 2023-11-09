using Newtonsoft.Json;

namespace DetectDupeItem.Services
{
    internal class QueryPayload
    {
        [JsonProperty("sql")]
        public string Sql { get; set; }
        [JsonProperty("payload")]
        public object Payload { get; set; }
    }
}
