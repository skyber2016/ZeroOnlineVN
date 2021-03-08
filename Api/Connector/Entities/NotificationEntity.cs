using System.ComponentModel.DataAnnotations.Schema;

namespace Forum_API.Entities
{
    [Table("Notification")]
    public class NotificationEntity : BaseEntity
    {
        public string Message { get; set; }
        public bool Seen { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }

        public virtual UserEntity User { get; set; }
    }
}
