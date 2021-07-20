using SqlKata;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using ColumnAttribute = SqlKata.ColumnAttribute;

namespace API.Entities
{
    [Table("reward_shop")]
    public class RewardShopEntity : BaseEntity
    {
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("item_id")]
        public int ItemId { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
