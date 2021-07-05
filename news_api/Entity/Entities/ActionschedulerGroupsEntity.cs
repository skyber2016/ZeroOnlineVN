using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_actionscheduler_groups")]
    public class ActionschedulerGroupsEntity : BaseEntity
    {
		[Key]
		[Column("group_id")]
		public string GroupId { get; set; }

		[Column("slug")]
		public string Slug { get; set; }


    }
}
