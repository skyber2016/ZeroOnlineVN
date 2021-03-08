using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum_API.Entities
{
    [Table("Area")]
    public class AreaEntity : BaseEntity
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public virtual ICollection<CategoryEntity> Categories { get; set; }
    }
}
