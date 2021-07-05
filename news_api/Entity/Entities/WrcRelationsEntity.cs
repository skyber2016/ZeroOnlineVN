using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_wrc_relations")]
    public class WrcRelationsEntity : BaseEntity
    {
		[Key]
		[Column("cache_id")]
		public string CacheId { get; set; }

		[Column("object_id")]
		public string ObjectId { get; set; }

		[Column("object_type")]
		public string ObjectType { get; set; }


    }
}
