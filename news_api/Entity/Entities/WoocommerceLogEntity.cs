using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_woocommerce_log")]
    public class WoocommerceLogEntity : BaseEntity
    {
		[Key]
		[Column("log_id")]
		public string LogId { get; set; }

		[Column("timestamp")]
		public DateTime Timestamp { get; set; }

		[Column("level")]
		public string Level { get; set; }

		[Column("source")]
		public string Source { get; set; }

		[Column("message")]
		public string Message { get; set; }

		[Column("context")]
		public string Context { get; set; }


    }
}
