using SqlKata;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using ColumnAttribute = SqlKata.ColumnAttribute;

namespace API.Entities
{
    [Table("reward_shop_item")]
    public class RewardShopItemEntity : BaseEntity
    {
        [Column("action_id")]
        public int ActionId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("image")]
        public string Image { get; set; }
        [Column("group")]
        public int Group { get; set; }
        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
