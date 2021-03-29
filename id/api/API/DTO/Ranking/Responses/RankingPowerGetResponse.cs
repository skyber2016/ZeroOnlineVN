using System.Text.Json.Serialization;

namespace API.DTO.Ranking.Responses
{
    public class RankingPowerGetResponse
    {
        public string Name { get; set; }
        [JsonIgnore]
        public int BattleLev { get; set; }
        [JsonPropertyName("battleLev")]
        public string BattleLevStr
        {
            get
            {
                return this.BattleLev.ToString("#,##0");
            }
        }
        public int Vip { get; set; }
        [JsonIgnore]
        public long Donation { get; set; }
        [JsonPropertyName("donation")]
        public string DonationStr
        {
            get
            {
                return this.Donation.ToString("#,##0");
            }
        }
    }
}
