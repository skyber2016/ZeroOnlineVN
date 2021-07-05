using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_woocommerce_payment_tokenmeta")]
    public class WoocommercePaymentTokenmetaEntity : BaseEntity
    {
		[Key]
		[Column("meta_id")]
		public string MetaId { get; set; }

		[Column("payment_token_id")]
		public string PaymentTokenId { get; set; }

		[Column("meta_key")]
		public string MetaKey { get; set; }

		[Column("meta_value")]
		public string MetaValue { get; set; }


    }
}
