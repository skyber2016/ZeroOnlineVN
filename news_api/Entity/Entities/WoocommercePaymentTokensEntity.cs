using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_woocommerce_payment_tokens")]
    public class WoocommercePaymentTokensEntity : BaseEntity
    {
		[Key]
		[Column("token_id")]
		public string TokenId { get; set; }

		[Column("gateway_id")]
		public string GatewayId { get; set; }

		[Column("token")]
		public string Token { get; set; }

		[Column("user_id")]
		public string UserId { get; set; }

		[Column("type")]
		public string Type { get; set; }

		[Column("is_default")]
		public string IsDefault { get; set; }


    }
}
