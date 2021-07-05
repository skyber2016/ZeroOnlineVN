using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_commentmeta")]
    public class CommentmetaEntity : BaseEntity
    {
		[Key]
		[Column("meta_id")]
		public string MetaId { get; set; }

		[Column("comment_id")]
		public string CommentId { get; set; }

		[Column("meta_key")]
		public string MetaKey { get; set; }

		[Column("meta_value")]
		public string MetaValue { get; set; }


    }
}
