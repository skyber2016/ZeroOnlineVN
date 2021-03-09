using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cambopay_API.Entities
{
    [Table("MailSending")]
    public class MailSendingEntity : BaseEntity
    {
        [ForeignKey("User")]
        public int UserId { get; set; }
        public string Template { get; set; }
        public int Status { get; set; }
        public DateTime? SentTime { get; set; }
        public string Title { get; set; }
        public string From { get; set; }
        public virtual UserEntity User { get; set; }
    }
}
