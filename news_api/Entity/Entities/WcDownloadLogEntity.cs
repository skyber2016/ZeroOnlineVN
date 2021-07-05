using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_wc_download_log")]
    public class WcDownloadLogEntity : BaseEntity
    {
		[Key]
		[Column("download_log_id")]
		public string DownloadLogId { get; set; }

		[Column("timestamp")]
		public DateTime Timestamp { get; set; }

		[Column("permission_id")]
		public string PermissionId { get; set; }

		[Column("user_id")]
		public string UserId { get; set; }

		[Column("user_ip_address")]
		public string UserIpAddress { get; set; }


    }
}
