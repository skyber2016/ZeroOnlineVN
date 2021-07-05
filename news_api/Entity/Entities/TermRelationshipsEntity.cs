using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_term_relationships")]
    public class TermRelationshipsEntity : BaseEntity
    {
		[Key]
		[Column("object_id")]
		[ForeignKey("Posts")]
		public string ObjectId { get; set; }

		[Column("term_taxonomy_id")]
		[ForeignKey("TermTaxonomy")]
		public string TermTaxonomyId { get; set; }

		[Column("term_order")]
		public int? TermOrder { get; set; }

		public virtual PostsEntity Posts { get; set; }
		public virtual TermTaxonomyEntity TermTaxonomy { get; set; }

	}
}
