using System;
using System.ComponentModel.DataAnnotations.Schema;
using ColumnAttribute = SqlKata.ColumnAttribute;
namespace API.Entities
{
    [Table("webshop_item")]
    public class WebShopItemEntity : BaseEntity
    {
        [Column("qty")]
        public int Qty { get; set; }
        
        [Column("name")]
        public string Name { get; set; }
        
        [Column("image")]
        public string Image { get; set; }
        
        [Column("price")]
        public int Price { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;


    }
}
