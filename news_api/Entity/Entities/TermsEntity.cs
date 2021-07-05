using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
	[Table("wp_terms")]
	public class TermsEntity : BaseEntity
	{
		[Key]
		[Column("term_id")]
		public string TermId { get; set; }

		[Column("name")]
		public string Name { get; set; }

		[Column("slug")]
		public string Slug { get; set; }

		[Column("term_group")]
		public string TermGroup { get; set; }

		[Column("term_order")]
		public int? TermOrder { get; set; }

	}
}