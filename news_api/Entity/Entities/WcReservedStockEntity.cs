using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_wc_reserved_stock")]
    public class WcReservedStockEntity : BaseEntity
    {
		[Key]
		[Column("order_id")]
		public string OrderId { get; set; }

		[Column("product_id")]
		public string ProductId { get; set; }

		[Column("stock_quantity")]
		public string StockQuantity { get; set; }

		[Column("timestamp")]
		public DateTime Timestamp { get; set; }

		[Column("expires")]
		public DateTime Expires { get; set; }


    }
}
