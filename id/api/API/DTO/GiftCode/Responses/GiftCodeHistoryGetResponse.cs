using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SqlKata;
using ColumnAttribute = SqlKata.ColumnAttribute;

namespace API.DTO.GiftCode.Responses
{
    [Table("gift_code_log")]
    public class GiftCodeHistoryGetResponse
    {
        [Column("code")]
        public string Code { get; set; }
        [Column("created_date")]
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }


        [JsonPropertyName("createdDate")]
        public string CreatedDateStr
        {
            get
            {
                return this.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss");
            }
        }
    }
}
