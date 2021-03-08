using System.ComponentModel.DataAnnotations.Schema;

namespace Forum_API.Entities
{
    [Table("SystemMessage")]
    public class SystemMessageEntity : BaseEntity
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string LanguageCode { get; set; }
    }
}
