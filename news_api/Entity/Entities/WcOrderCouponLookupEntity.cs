using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_wc_order_coupon_lookup")]
    public class WcOrderCouponLookupEntity : BaseEntity
    {
		[Key]
		[Column("order_id")]
		public string OrderId { get; set; }

		[Column("coupon_id")]
		public string CouponId { get; set; }

		[Column("date_created")]
		public DateTime DateCreated { get; set; }

		[Column("discount_amount")]
		public string DiscountAmount { get; set; }


    }
}
