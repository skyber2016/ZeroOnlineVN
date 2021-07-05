using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_termmeta")]
    public class TermmetaEntity : BaseEntity
    {
		[Key]
		[Column("meta_id")]
		public string MetaId { get; set; }

		[Column("term_id")]
		public string TermId { get; set; }

		[Column("meta_key")]
		public string MetaKey { get; set; }

		[Column("meta_value")]
		public string MetaValue { get; set; }


    }
}
