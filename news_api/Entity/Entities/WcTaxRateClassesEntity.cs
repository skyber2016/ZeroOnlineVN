using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_wc_tax_rate_classes")]
    public class WcTaxRateClassesEntity : BaseEntity
    {
		[Key]
		[Column("tax_rate_class_id")]
		public string TaxRateClassId { get; set; }

		[Column("name")]
		public string Name { get; set; }

		[Column("slug")]
		public string Slug { get; set; }


    }
}
