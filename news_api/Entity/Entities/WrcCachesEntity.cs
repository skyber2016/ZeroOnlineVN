using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_wrc_caches")]
    public class WrcCachesEntity : BaseEntity
    {
		[Key]
		[Column("cache_id")]
		public string CacheId { get; set; }

		[Column("cache_key")]
		public string CacheKey { get; set; }

		[Column("cache_type")]
		public string CacheType { get; set; }

		[Column("request_uri")]
		public string RequestUri { get; set; }

		[Column("request_headers")]
		public string RequestHeaders { get; set; }

		[Column("request_method")]
		public string RequestMethod { get; set; }

		[Column("object_type")]
		public string ObjectType { get; set; }

		[Column("cache_hits")]
		public string CacheHits { get; set; }

		[Column("is_single")]
		public string IsSingle { get; set; }

		[Column("expiration")]
		public DateTime Expiration { get; set; }

		[Column("deleted")]
		public string Deleted { get; set; }

		[Column("cleaned")]
		public string Cleaned { get; set; }


    }
}
