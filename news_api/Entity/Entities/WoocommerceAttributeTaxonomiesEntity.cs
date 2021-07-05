using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_woocommerce_attribute_taxonomies")]
    public class WoocommerceAttributeTaxonomiesEntity : BaseEntity
    {
		[Key]
		[Column("attribute_id")]
		public string AttributeId { get; set; }

		[Column("attribute_name")]
		public string AttributeName { get; set; }

		[Column("attribute_label")]
		public string AttributeLabel { get; set; }

		[Column("attribute_type")]
		public string AttributeType { get; set; }

		[Column("attribute_orderby")]
		public string AttributeOrderby { get; set; }

		[Column("attribute_public")]
		public int AttributePublic { get; set; }


    }
}
