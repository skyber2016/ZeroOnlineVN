using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Forum_API.Entities
{
    [Table("Category")]
    public class CategoryEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPinned { get; set; }
        [ForeignKey("Area")]
        public long AreaId { get; set; }

        [ForeignKey("Parent")]
        public long? ParentId { get; set; }

        public long Views { get; set; }

        public virtual ICollection<TopicEntity> Topics { get; set; }
        public virtual AreaEntity Area { get; set; }
        public virtual CategoryEntity Parent { get; set; }

        public virtual ICollection<CategoryEntity> Childrens { get; set; }
        public int TotalTopics
        {
            get
            {
                return this.Topics.Count;
            }
        }

        public TopicEntity LastTopic
        {
            get
            {
                return this.Topics.LastOrDefault();
            }
        }
    }
}
