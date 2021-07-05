using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_wc_order_stats")]
    public class WcOrderStatsEntity : BaseEntity
    {
		[Key]
		[Column("order_id")]
		public string OrderId { get; set; }

		[Column("parent_id")]
		public string ParentId { get; set; }

		[Column("date_created")]
		public DateTime DateCreated { get; set; }

		[Column("date_created_gmt")]
		public DateTime DateCreatedGmt { get; set; }

		[Column("num_items_sold")]
		public int NumItemsSold { get; set; }

		[Column("total_sales")]
		public string TotalSales { get; set; }

		[Column("tax_total")]
		public string TaxTotal { get; set; }

		[Column("shipping_total")]
		public string ShippingTotal { get; set; }

		[Column("net_total")]
		public string NetTotal { get; set; }

		[Column("returning_customer")]
		public string ReturningCustomer { get; set; }

		[Column("status")]
		public string Status { get; set; }

		[Column("customer_id")]
		public string CustomerId { get; set; }


    }
}
