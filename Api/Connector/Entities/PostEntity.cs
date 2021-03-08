using System.ComponentModel.DataAnnotations.Schema;

namespace Forum_API.Entities
{
    [Table("Post")]
    public class PostEntity : BaseEntity
    {
        public string Content { get; set; }
        [ForeignKey("Topic")]
        public long TopicId { get; set; }
        public virtual TopicEntity Topic { get; set; }
    }
}
