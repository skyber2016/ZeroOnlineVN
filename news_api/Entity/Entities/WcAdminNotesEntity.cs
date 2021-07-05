using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_wc_admin_notes")]
    public class WcAdminNotesEntity : BaseEntity
    {
		[Key]
		[Column("note_id")]
		public string NoteId { get; set; }

		[Column("name")]
		public string Name { get; set; }

		[Column("type")]
		public string Type { get; set; }

		[Column("locale")]
		public string Locale { get; set; }

		[Column("title")]
		public string Title { get; set; }

		[Column("content")]
		public string Content { get; set; }

		[Column("content_data")]
		public string ContentData { get; set; }

		[Column("status")]
		public string Status { get; set; }

		[Column("source")]
		public string Source { get; set; }

		[Column("date_created")]
		public DateTime DateCreated { get; set; }

		[Column("date_reminder")]
		public DateTime? DateReminder { get; set; }

		[Column("is_snoozable")]
		public string IsSnoozable { get; set; }

		[Column("layout")]
		public string Layout { get; set; }

		[Column("image")]
		public string Image { get; set; }

		[Column("is_deleted")]
		public string IsDeleted { get; set; }

		[Column("icon")]
		public string Icon { get; set; }


    }
}
