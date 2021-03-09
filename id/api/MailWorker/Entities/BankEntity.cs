using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cambopay_API.Entities
{
    [Table("Bank")]
    public class BankEntity : BaseEntity
    {
        [StringLength(200)]
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [StringLength(200)]
        [MaxLength(6)]
        [Required]
        public string Code { get; set; }


    }
}
