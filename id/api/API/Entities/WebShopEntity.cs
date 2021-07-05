using System;
using System.ComponentModel.DataAnnotations.Schema;
using ColumnAttribute = SqlKata.ColumnAttribute;
namespace API.Entities
{
    [Table("webshop")]
    public class WebShopEntity : BaseEntity
    {
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("char_name")]
        public string CharName { get; set; }
        
        [Column("item_id")]
        public int ItemId { get; set; }

        [Column("item_name")]
        public string ItemName { get; set; }

        [Column("price")]
        public int Price { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
