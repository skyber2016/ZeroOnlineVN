using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_woocommerce_tax_rates")]
    public class WoocommerceTaxRatesEntity : BaseEntity
    {
		[Key]
		[Column("tax_rate_id")]
		public string TaxRateId { get; set; }

		[Column("tax_rate_country")]
		public string TaxRateCountry { get; set; }

		[Column("tax_rate_state")]
		public string TaxRateState { get; set; }

		[Column("tax_rate")]
		public string TaxRate { get; set; }

		[Column("tax_rate_name")]
		public string TaxRateName { get; set; }

		[Column("tax_rate_priority")]
		public string TaxRatePriority { get; set; }

		[Column("tax_rate_compound")]
		public int TaxRateCompound { get; set; }

		[Column("tax_rate_shipping")]
		public int TaxRateShipping { get; set; }

		[Column("tax_rate_order")]
		public string TaxRateOrder { get; set; }

		[Column("tax_rate_class")]
		public string TaxRateClass { get; set; }


    }
}
