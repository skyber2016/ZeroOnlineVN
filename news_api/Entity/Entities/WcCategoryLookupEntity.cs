using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_wc_category_lookup")]
    public class WcCategoryLookupEntity : BaseEntity
    {
		[Key]
		[Column("category_tree_id")]
		public string CategoryTreeId { get; set; }

		[Column("category_id")]
		public string CategoryId { get; set; }


    }
}
