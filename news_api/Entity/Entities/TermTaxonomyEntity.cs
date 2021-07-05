using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_term_taxonomy")]
    public class TermTaxonomyEntity : BaseEntity
    {
		[Key]
		[Column("term_taxonomy_id")]
		public string TermTaxonomyId { get; set; }

		[Column("term_id")]
		[ForeignKey("Term")]
		public string TermId { get; set; }

		[Column("taxonomy")]
		public string Taxonomy { get; set; }

		[Column("description")]
		public string Description { get; set; }

		[Column("parent")]
		public string Parent { get; set; }

		[Column("count")]
		public string Count { get; set; }

		public virtual TermsEntity Term { get; set; }
	}
}
