using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cambopay_API.Entities
{
    [Table("QRType")]
    public class QRTypeEntity : BaseEntity
    {
        [StringLength(500)]
        [Required]
        public string Name { get; set; }
        [StringLength(500)]
        [Required]
        public string Code { get; set; }
    }
}
