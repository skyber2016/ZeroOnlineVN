using SqlKata;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using ColumnAttribute = SqlKata.ColumnAttribute;
namespace API.Entities
{
    [Table("gift_code_log")]
    public class GiftCodeLogEntity : BaseEntity
    {
        [Column("gift_code_id")]
        public int GiftCodeId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
