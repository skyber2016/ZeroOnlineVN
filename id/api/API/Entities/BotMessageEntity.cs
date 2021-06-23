using API.Common;
using SqlKata;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using ColumnAttribute = SqlKata.ColumnAttribute;

namespace API.Entities
{
    [Table("bot_message")]
    public class BotMessageEntity : BaseEntity
    {
        public BotMessageEntity()
        {
            this.CreatedDate = DateTime.Now;
            this.Channel = ChannelConstant.REPORT.ToString();
        }

        [Column("message")]
        public string Message { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("is_sent")]
        public bool IsSent { get; set; }

        [Column("image")]
        public string Image { get; set; }

        [Column("channel")]
        public string Channel { get; set; }
    }
}
