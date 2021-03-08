using System.ComponentModel.DataAnnotations.Schema;

namespace Forum_API.Entities
{
    [Table("SystemConfig")]
    public class SystemConfigEntity : BaseEntity
    {
        public string Code { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
    }
}
