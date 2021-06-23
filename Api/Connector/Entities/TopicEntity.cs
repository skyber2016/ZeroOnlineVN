using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Forum_API.Entities
{
    [Table("Topic")]
    public class TopicEntity : BaseEntity
    {
        public string Name { get; set; }
        public int Status { get; set; }
        public int Views { get; set; }
        public bool IsPinned { get; set; }
        [ForeignKey("Categories")]
        public long CategoryId { get; set; }
        public bool IsAnnouncement { get; set; }
        public virtual CategoryEntity Category { get; set; }
        public virtual ICollection<PostEntity> Posts { get; set; }

    }
}
