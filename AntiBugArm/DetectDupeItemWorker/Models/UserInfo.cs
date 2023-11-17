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

        public bool AccountHasBanned()
        {
            return IsBanned != 0;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject((object)this);
        }

        public async Task<bool> DoBannedAccount()
        {
            QueryPayload queryPayload = new QueryPayload();
            queryPayload.Sql = "UPDATE account SET ONLINE = 1 WHERE id = ?";
            queryPayload.Payload = new object[1] { AccountId };
            bool num = await DatabaseService.ExecuteNonResult(queryPayload);
            if (num)
            {
                Task.Run(async delegate
               {
                   await WinService.BlockIP(IP);
                   await Task.Delay(60000 * 2);
                   await WinService.UnblockIP(IP);
               });
            }
            return num;
        }
    }


}
