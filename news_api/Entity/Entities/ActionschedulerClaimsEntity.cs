using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_actionscheduler_claims")]
    public class ActionschedulerClaimsEntity : BaseEntity
    {
		[Key]
		[Column("claim_id")]
		public string ClaimId { get; set; }

		[Column("date_created_gmt")]
		public DateTime DateCreatedGmt { get; set; }


    }
}
