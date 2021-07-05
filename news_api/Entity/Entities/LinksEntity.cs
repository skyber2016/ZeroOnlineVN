using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_links")]
    public class LinksEntity : BaseEntity
    {
		[Key]
		[Column("link_id")]
		public string LinkId { get; set; }

		[Column("link_url")]
		public string LinkUrl { get; set; }

		[Column("link_name")]
		public string LinkName { get; set; }

		[Column("link_image")]
		public string LinkImage { get; set; }

		[Column("link_target")]
		public string LinkTarget { get; set; }

		[Column("link_description")]
		public string LinkDescription { get; set; }

		[Column("link_visible")]
		public string LinkVisible { get; set; }

		[Column("link_owner")]
		public string LinkOwner { get; set; }

		[Column("link_rating")]
		public int LinkRating { get; set; }

		[Column("link_updated")]
		public DateTime LinkUpdated { get; set; }

		[Column("link_rel")]
		public string LinkRel { get; set; }

		[Column("link_notes")]
		public string LinkNotes { get; set; }

		[Column("link_rss")]
		public string LinkRss { get; set; }


    }
}
