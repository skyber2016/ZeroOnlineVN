using Newtonsoft.Json;

namespace DetectDupeItem.Models
{
    internal class ItemExEntity
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
