using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_actionscheduler_actions")]
    public class ActionschedulerActionsEntity : BaseEntity
    {
		[Key]
		[Column("action_id")]
		public string ActionId { get; set; }

		[Column("hook")]
		public string Hook { get; set; }

		[Column("status")]
		public string Status { get; set; }

		[Column("scheduled_date_gmt")]
		public DateTime ScheduledDateGmt { get; set; }

		[Column("scheduled_date_local")]
		public DateTime ScheduledDateLocal { get; set; }

		[Column("args")]
		public string Args { get; set; }

		[Column("schedule")]
		public string Schedule { get; set; }

		[Column("group_id")]
		public string GroupId { get; set; }

		[Column("attempts")]
		public int Attempts { get; set; }

		[Column("last_attempt_gmt")]
		public DateTime LastAttemptGmt { get; set; }

		[Column("last_attempt_local")]
		public DateTime LastAttemptLocal { get; set; }

		[Column("claim_id")]
		public string ClaimId { get; set; }

		[Column("extended_args")]
		public string ExtendedArgs { get; set; }


    }
}
