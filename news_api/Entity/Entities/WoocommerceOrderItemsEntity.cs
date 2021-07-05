using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_woocommerce_order_items")]
    public class WoocommerceOrderItemsEntity : BaseEntity
    {
		[Key]
		[Column("order_item_id")]
		public string OrderItemId { get; set; }

		[Column("order_item_name")]
		public string OrderItemName { get; set; }

		[Column("order_item_type")]
		public string OrderItemType { get; set; }

		[Column("order_id")]
		public string OrderId { get; set; }


    }
}
