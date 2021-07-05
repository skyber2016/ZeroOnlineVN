using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_woocommerce_shipping_zone_locations")]
    public class WoocommerceShippingZoneLocationsEntity : BaseEntity
    {
		[Key]
		[Column("location_id")]
		public string LocationId { get; set; }

		[Column("zone_id")]
		public string ZoneId { get; set; }

		[Column("location_code")]
		public string LocationCode { get; set; }

		[Column("location_type")]
		public string LocationType { get; set; }


    }
}
