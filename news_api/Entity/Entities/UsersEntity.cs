using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_users")]
    public class UsersEntity : BaseEntity
    {
		[Key]
		[Column("ID")]
		public string Id { get; set; }

		[Column("user_login")]
		public string UserLogin { get; set; }

		[Column("user_pass")]
		public string UserPass { get; set; }

		[Column("user_nicename")]
		public string UserNicename { get; set; }

		[Column("user_email")]
		public string UserEmail { get; set; }

		[Column("user_url")]
		public string UserUrl { get; set; }

		[Column("user_registered")]
		public DateTime UserRegistered { get; set; }

		[Column("user_activation_key")]
		public string UserActivationKey { get; set; }

		[Column("user_status")]
		public int UserStatus { get; set; }

		[Column("display_name")]
		public string DisplayName { get; set; }


    }
}
