using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_woocommerce_tax_rate_locations")]
    public class WoocommerceTaxRateLocationsEntity : BaseEntity
    {
		[Key]
		[Column("location_id")]
		public string LocationId { get; set; }

		[Column("location_code")]
		public string LocationCode { get; set; }

		[Column("tax_rate_id")]
		public string TaxRateId { get; set; }

		[Column("location_type")]
		public string LocationType { get; set; }


    }
}
