using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_postmeta")]
    public class PostmetaEntity : BaseEntity
    {
		[Key]
		[Column("meta_id")]
		public string MetaId { get; set; }

		[Column("post_id")]
		public string PostId { get; set; }

		[Column("meta_key")]
		public string MetaKey { get; set; }

		[Column("meta_value")]
		public string MetaValue { get; set; }


    }
}
