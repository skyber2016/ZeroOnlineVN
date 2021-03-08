using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum_API.Entities
{
    [Table("MailQueue")]
    public class MailQueueEntity : BaseEntity
    {
        public string Content { get; set; }
        public string Title { get; set; }
        public DateTime? SentTime { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        public virtual UserEntity User { get; set; }

    }
}
