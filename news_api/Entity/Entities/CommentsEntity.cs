using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_comments")]
    public class CommentsEntity : BaseEntity
    {
		[Key]
		[Column("comment_ID")]
		public string CommentId { get; set; }

		[Column("comment_post_ID")]
		public string CommentPostId { get; set; }

		[Column("comment_author")]
		public string CommentAuthor { get; set; }

		[Column("comment_author_email")]
		public string CommentAuthorEmail { get; set; }

		[Column("comment_author_url")]
		public string CommentAuthorUrl { get; set; }

		[Column("comment_author_IP")]
		public string CommentAuthorIp { get; set; }

		[Column("comment_date")]
		public DateTime CommentDate { get; set; }

		[Column("comment_date_gmt")]
		public DateTime CommentDateGmt { get; set; }

		[Column("comment_content")]
		public string CommentContent { get; set; }

		[Column("comment_karma")]
		public int CommentKarma { get; set; }

		[Column("comment_approved")]
		public string CommentApproved { get; set; }

		[Column("comment_agent")]
		public string CommentAgent { get; set; }

		[Column("comment_type")]
		public string CommentType { get; set; }

		[Column("comment_parent")]
		public string CommentParent { get; set; }

		[Column("user_id")]
		public string UserId { get; set; }


    }
}
