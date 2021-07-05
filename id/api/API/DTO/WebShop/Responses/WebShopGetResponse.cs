using System;
using System.Text.Json.Serialization;

namespace API.DTO.WebShop.Responses
{
    public class WebShopGetResponse
    {
        public string CharName { get; set; }

        public string ItemName { get; set; }

        [JsonIgnore]
        public DateTime CreatedDate { get; set; }

        [JsonPropertyName("createdDate")]
        public string CreatedDateStr
        {
            get
            {
                return this.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }
}
