using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_woocommerce_sessions")]
    public class WoocommerceSessionsEntity : BaseEntity
    {
		[Key]
		[Column("session_id")]
		public string SessionId { get; set; }

		[Column("session_key")]
		public char SessionKey { get; set; }

		[Column("session_value")]
		public string SessionValue { get; set; }

		[Column("session_expiry")]
		public string SessionExpiry { get; set; }


    }
}
