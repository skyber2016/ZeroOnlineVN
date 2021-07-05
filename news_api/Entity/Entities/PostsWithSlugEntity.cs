using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_posts")]
    public class PostsWithoutSlugEntity : BaseEntity
    {
		[Key]
		[Column("ID")]
		public string Id { get; set; }

		[Column("post_author")]
		public string PostAuthor { get; set; }

		[Column("post_date")]
		public DateTime PostDate { get; set; }

		[Column("post_content")]
		public string PostContent { get; set; }

		[Column("post_title")]
		public string PostTitle { get; set; }

		[Column("post_excerpt")]
		public string PostExcerpt { get; set; }

		[Column("post_status")]
		public string PostStatus { get; set; }

		[Column("comment_status")]
		public string CommentStatus { get; set; }

		[Column("ping_status")]
		public string PingStatus { get; set; }

		[Column("post_password")]
		public string PostPassword { get; set; }

		[Column("post_name")]
		public string PostName { get; set; }

		[Column("to_ping")]
		public string ToPing { get; set; }

		[Column("pinged")]
		public string Pinged { get; set; }

		[Column("post_modified")]
		public DateTime PostModified { get; set; }

		[Column("post_content_filtered")]
		public string PostContentFiltered { get; set; }

		[Column("post_parent")]
		public string PostParent { get; set; }

		[Column("guid")]
		public string Guid { get; set; }

		[Column("menu_order")]
		public int MenuOrder { get; set; }

		[Column("post_type")]
		public string PostType { get; set; }

		[Column("post_mime_type")]
		public string PostMimeType { get; set; }

		[Column("comment_count")]
		public string CommentCount { get; set; }

	}
}
