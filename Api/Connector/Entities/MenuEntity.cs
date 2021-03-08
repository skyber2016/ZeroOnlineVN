using System.ComponentModel.DataAnnotations.Schema;

namespace Forum_API.Entities
{
    [Table("Menu")]
    public class MenuEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public int Order { get; set; }
    }
}
