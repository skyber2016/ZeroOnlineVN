using Newtonsoft.Json;
using System;

namespace DetectDupeItem.Models
{
    internal class BotMessageEntity
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("is_sent")]
        public bool IsSent { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("channel")]
        public string Channel { get; set; }
    }
}
