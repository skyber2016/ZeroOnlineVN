using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_woocommerce_order_itemmeta")]
    public class WoocommerceOrderItemmetaEntity : BaseEntity
    {
		[Key]
		[Column("meta_id")]
		public string MetaId { get; set; }

		[Column("order_item_id")]
		public string OrderItemId { get; set; }

		[Column("meta_key")]
		public string MetaKey { get; set; }

		[Column("meta_value")]
		public string MetaValue { get; set; }


    }
}
