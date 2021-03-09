using System.Text.Json.Serialization;

namespace API.DTO.Base
{
    public class StatusResponse
    {
        [JsonIgnore]
        public char IsEnabled { get; set; }
        public bool Enabled
        {
            get
            {
                return this.IsEnabled == '1';
            }
        }

        public string StatusName
        {
            get
            {
                if (this.Enabled)
                {
                    return "Hoạt động";
                }
                return "Đóng";
            }
        }
    }
}
