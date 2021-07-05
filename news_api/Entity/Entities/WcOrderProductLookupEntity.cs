using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_wc_order_product_lookup")]
    public class WcOrderProductLookupEntity : BaseEntity
    {
		[Key]
		[Column("order_item_id")]
		public string OrderItemId { get; set; }

		[Column("order_id")]
		public string OrderId { get; set; }

		[Column("product_id")]
		public string ProductId { get; set; }

		[Column("variation_id")]
		public string VariationId { get; set; }

		[Column("customer_id")]
		public string CustomerId { get; set; }

		[Column("date_created")]
		public DateTime DateCreated { get; set; }

		[Column("product_qty")]
		public int ProductQty { get; set; }

		[Column("product_net_revenue")]
		public string ProductNetRevenue { get; set; }

		[Column("product_gross_revenue")]
		public string ProductGrossRevenue { get; set; }

		[Column("coupon_amount")]
		public string CouponAmount { get; set; }

		[Column("tax_amount")]
		public string TaxAmount { get; set; }

		[Column("shipping_amount")]
		public string ShippingAmount { get; set; }

		[Column("shipping_tax_amount")]
		public string ShippingTaxAmount { get; set; }


    }
}
