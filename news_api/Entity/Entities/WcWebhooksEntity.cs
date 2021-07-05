using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_wc_webhooks")]
    public class WcWebhooksEntity : BaseEntity
    {
		[Key]
		[Column("webhook_id")]
		public string WebhookId { get; set; }

		[Column("status")]
		public string Status { get; set; }

		[Column("name")]
		public string Name { get; set; }

		[Column("user_id")]
		public string UserId { get; set; }

		[Column("delivery_url")]
		public string DeliveryUrl { get; set; }

		[Column("secret")]
		public string Secret { get; set; }

		[Column("topic")]
		public string Topic { get; set; }

		[Column("date_created")]
		public DateTime DateCreated { get; set; }

		[Column("date_created_gmt")]
		public DateTime DateCreatedGmt { get; set; }

		[Column("date_modified")]
		public DateTime DateModified { get; set; }

		[Column("date_modified_gmt")]
		public DateTime DateModifiedGmt { get; set; }

		[Column("api_version")]
		public string ApiVersion { get; set; }

		[Column("failure_count")]
		public string FailureCount { get; set; }

		[Column("pending_delivery")]
		public string PendingDelivery { get; set; }


    }
}
