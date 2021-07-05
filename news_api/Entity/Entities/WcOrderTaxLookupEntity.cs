using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_wc_order_tax_lookup")]
    public class WcOrderTaxLookupEntity : BaseEntity
    {
		[Column("order_id")]
		public string OrderId { get; set; }

		[Key]
		[Column("tax_rate_id")]
		public string TaxRateId { get; set; }

		[Column("date_created")]
		public DateTime DateCreated { get; set; }

		[Column("shipping_tax")]
		public string ShippingTax { get; set; }

		[Column("order_tax")]
		public string OrderTax { get; set; }

		[Column("total_tax")]
		public string TotalTax { get; set; }


    }
}
