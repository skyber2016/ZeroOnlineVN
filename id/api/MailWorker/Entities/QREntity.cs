using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cambopay_API.Entities
{
    [Table("QR")]
    public class QREntity : BaseEntity
    {
        [StringLength(500)]
        [Required]
        public string FullName { get; set; }
        [StringLength(500)]
        [Required]
        public string AccountNumber { get; set; }
        [ForeignKey("Bank")]
        [Required]
        public int BankId { get; set; }
        public string Note { get; set; }
        [ForeignKey("QRType")]
        [Required]
        public int QRTypeId { get; set; }
        public virtual BankEntity Bank { get; set; }
        public virtual QRTypeEntity QRType { get; set; }
    }
}
