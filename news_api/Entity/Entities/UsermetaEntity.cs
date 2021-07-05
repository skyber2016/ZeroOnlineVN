using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_usermeta")]
    public class UsermetaEntity : BaseEntity
    {
		[Key]
		[Column("umeta_id")]
		public string UmetaId { get; set; }

		[Column("user_id")]
		public string UserId { get; set; }

		[Column("meta_key")]
		public string MetaKey { get; set; }

		[Column("meta_value")]
		public string MetaValue { get; set; }


    }
}
