using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_woocommerce_shipping_zones")]
    public class WoocommerceShippingZonesEntity : BaseEntity
    {
		[Key]
		[Column("zone_id")]
		public string ZoneId { get; set; }

		[Column("zone_name")]
		public string ZoneName { get; set; }

		[Column("zone_order")]
		public string ZoneOrder { get; set; }


    }
}
