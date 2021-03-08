using System.ComponentModel.DataAnnotations.Schema;

namespace Forum_API.Entities
{
    [Table("File")]
    public class FileEntity : BaseEntity
    {
        public string PhysicalName { get; set; }
        public string FileName { get; set; }
        public long Size { get; set; }
        public string Ext { get; set; }
        public string ContentType { get; set; }
    }
}
