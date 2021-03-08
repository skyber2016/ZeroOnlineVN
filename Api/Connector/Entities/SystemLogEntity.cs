using System.ComponentModel.DataAnnotations.Schema;

namespace Forum_API.Entities
{
    [Table("SystemLog")]
    public class SystemLogEntity : BaseEntity
    {
        public string Content { get; set; }
        public string Type { get; set; }
    }
}
