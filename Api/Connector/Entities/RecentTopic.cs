using System.ComponentModel.DataAnnotations.Schema;

namespace Forum_API.Entities
{
    public class RecentTopic : BaseEntity
    {
        [ForeignKey("User")]
        public long UserId { get; set; }
        [ForeignKey("Topic")]
        public long TopicId { get; set; }

        public virtual UserEntity User { get; set; }
        public virtual TopicEntity Topic { get; set; }
    }
}
