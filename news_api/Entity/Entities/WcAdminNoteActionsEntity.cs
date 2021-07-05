using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("wp_wc_admin_note_actions")]
    public class WcAdminNoteActionsEntity : BaseEntity
    {
		[Key]
		[Column("action_id")]
		public string ActionId { get; set; }

		[Column("note_id")]
		public string NoteId { get; set; }

		[Column("name")]
		public string Name { get; set; }

		[Column("label")]
		public string Label { get; set; }

		[Column("query")]
		public string Query { get; set; }

		[Column("status")]
		public string Status { get; set; }

		[Column("is_primary")]
		public string IsPrimary { get; set; }

		[Column("actioned_text")]
		public string ActionedText { get; set; }


    }
}
