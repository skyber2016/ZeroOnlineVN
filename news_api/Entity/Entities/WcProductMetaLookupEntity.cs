using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_wc_product_meta_lookup")]
    public class WcProductMetaLookupEntity : BaseEntity
    {
		[Key]
		[Column("product_id")]
		public string ProductId { get; set; }

		[Column("sku")]
		public string Sku { get; set; }

		[Column("virtual")]
		public string Virtual { get; set; }

		[Column("downloadable")]
		public string Downloadable { get; set; }

		[Column("min_price")]
		public string MinPrice { get; set; }

		[Column("max_price")]
		public string MaxPrice { get; set; }

		[Column("onsale")]
		public string Onsale { get; set; }

		[Column("stock_quantity")]
		public string StockQuantity { get; set; }

		[Column("stock_status")]
		public string StockStatus { get; set; }

		[Column("rating_count")]
		public string RatingCount { get; set; }

		[Column("average_rating")]
		public string AverageRating { get; set; }

		[Column("total_sales")]
		public string TotalSales { get; set; }

		[Column("tax_status")]
		public string TaxStatus { get; set; }

		[Column("tax_class")]
		public string TaxClass { get; set; }


    }
}
