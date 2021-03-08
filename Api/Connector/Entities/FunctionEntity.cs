using System.ComponentModel.DataAnnotations.Schema;

namespace Forum_API.Entities
{
    [Table("Function")]
    public class FunctionEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsPage { get; set; }
    }
}
