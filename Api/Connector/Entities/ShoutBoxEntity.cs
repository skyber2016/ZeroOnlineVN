using System.ComponentModel.DataAnnotations.Schema;

namespace Forum_API.Entities
{
    [Table("ShoutBox")]
    public class ShoutBoxEntity : BaseEntity
    {
        public string Message { get; set; }
    }
}
