using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_woocommerce_shipping_zone_methods")]
    public class WoocommerceShippingZoneMethodsEntity : BaseEntity
    {
		[Column("zone_id")]
		public string ZoneId { get; set; }

		[Key]
		[Column("instance_id")]
		public string InstanceId { get; set; }

		[Column("method_id")]
		public string MethodId { get; set; }

		[Column("method_order")]
		public string MethodOrder { get; set; }

		[Column("is_enabled")]
		public string IsEnabled { get; set; }


    }
}
