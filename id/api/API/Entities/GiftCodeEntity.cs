using System;
using System.ComponentModel.DataAnnotations.Schema;
using ColumnAttribute = SqlKata.ColumnAttribute;
namespace API.Entities
{
    [Table("gift_code")]
    public class GiftCodeEntity : BaseEntity
    {
        [Column("code")]
        public string Code { get; set; }

        [Column("type")]
        public int Type { get; set; }

        [Column("item_id")]
        public int ItemId { get; set; }

        [Column("is_used")]
        public bool IsUsed { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
