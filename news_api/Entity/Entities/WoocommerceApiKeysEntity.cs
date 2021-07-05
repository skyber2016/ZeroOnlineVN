using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_woocommerce_api_keys")]
    public class WoocommerceApiKeysEntity : BaseEntity
    {
		[Key]
		[Column("key_id")]
		public string KeyId { get; set; }

		[Column("user_id")]
		public string UserId { get; set; }

		[Column("description")]
		public string Description { get; set; }

		[Column("permissions")]
		public string Permissions { get; set; }

		[Column("consumer_key")]
		public char ConsumerKey { get; set; }

		[Column("consumer_secret")]
		public char ConsumerSecret { get; set; }

		[Column("nonces")]
		public string Nonces { get; set; }

		[Column("truncated_key")]
		public char TruncatedKey { get; set; }

		[Column("last_access")]
		public DateTime? LastAccess { get; set; }


    }
}
