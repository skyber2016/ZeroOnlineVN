using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_actionscheduler_logs")]
    public class ActionschedulerLogsEntity : BaseEntity
    {
		[Key]
		[Column("log_id")]
		public string LogId { get; set; }

		[Column("action_id")]
		public string ActionId { get; set; }

		[Column("message")]
		public string Message { get; set; }

		[Column("log_date_gmt")]
		public DateTime LogDateGmt { get; set; }

		[Column("log_date_local")]
		public DateTime LogDateLocal { get; set; }


    }
}
