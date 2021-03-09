using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cambopay_API.Entities
{
    [Table("Page")]
    public class PageEntity : BaseEntity
    {
        [StringLength(500)]
        public string AliasPath { get; set; }
        public int Order { get; set; }

    }
}
