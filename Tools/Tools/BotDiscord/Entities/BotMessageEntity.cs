using System;
using System.ComponentModel.DataAnnotations.Schema;
using SqlKata;
using ColumnAttribute = SqlKata.ColumnAttribute;

namespace API.Entities
{
    [Table("bot_message")]
    public class BotMessageEntity : BaseEntity
    {
        public BotMessageEntity()
        {
            this.CreatedDate = DateTime.Now;
        }

        [Column("message")]
        public string Message { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("is_sent")]
        public bool IsSent { get; set; }
        [Column("image")]
        public string Image { get; set; }
    }
}
