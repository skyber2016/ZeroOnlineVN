using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_woocommerce_downloadable_product_permissions")]
    public class WoocommerceDownloadableProductPermissionsEntity : BaseEntity
    {
		[Key]
		[Column("permission_id")]
		public string PermissionId { get; set; }

		[Column("download_id")]
		public string DownloadId { get; set; }

		[Column("product_id")]
		public string ProductId { get; set; }

		[Column("order_id")]
		public string OrderId { get; set; }

		[Column("order_key")]
		public string OrderKey { get; set; }

		[Column("user_email")]
		public string UserEmail { get; set; }

		[Column("user_id")]
		public string UserId { get; set; }

		[Column("downloads_remaining")]
		public string DownloadsRemaining { get; set; }

		[Column("access_granted")]
		public DateTime AccessGranted { get; set; }

		[Column("access_expires")]
		public DateTime? AccessExpires { get; set; }

		[Column("download_count")]
		public string DownloadCount { get; set; }


    }
}
