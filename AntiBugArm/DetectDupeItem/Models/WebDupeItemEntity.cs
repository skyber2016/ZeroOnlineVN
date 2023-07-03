using Newtonsoft.Json;
using System;

namespace DetectDupeItem.Models
{
    internal class WebDupeItemEntity
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("player_name")]
        public string PlayerName { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("user_id")]
        public long UserId { get; set; }

        [JsonProperty("account_id")]
        public long AccountId { get; set; }

        [JsonProperty("robot_id")]
        public long RobotId { get; set; }

        [JsonProperty("source_id")]
        public long SourceId { get; set; }

        [JsonProperty("source_type")]
        public long SourceType { get; set; }

        [JsonProperty("dest_id")]
        public long DestId { get; set; }

        [JsonProperty("dest_type")]
        public long DestType { get; set; }

        [JsonProperty("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("line_number")]
        public int LineNumber { get; set; }

        [JsonProperty("file_name")]
        public string FileName { get; set; }

        public static string GetTableName() => "web_dupe_item";
    }
}
