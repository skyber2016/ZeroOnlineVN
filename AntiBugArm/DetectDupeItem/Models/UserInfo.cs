using DetectDupeItem.Services;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace DetectDupeItem.Models
{
    internal class UserInfo
    {
        [JsonProperty("player_name")]
        public string PlayerName { get; set; }

        [JsonProperty("account_id")]
        public long AccountId { get; set; }

        [JsonProperty("user_id")]
        public long UserId { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("robot_id")]
        public long RobotId { get; set; }

        [JsonProperty("is_banned")]
        public int IsBanned { get; set; }

        [JsonProperty("ip")]
        public string IP { get; set; }

        public bool AccountHasBanned() => this.IsBanned != 0;
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public async Task<bool> Banned()
        {
            var query = new QueryPayload
            {
                Sql = "UPDATE account SET ONLINE = 1 WHERE id = ?",
                Payload = new object[] { this.AccountId }
            };
            return true;
            //return await DatabaseService.ExecuteNonResult(query);
        }
    }

}
