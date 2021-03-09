using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cambopay_API.Entities
{
    [Table("ForgotPasswordRequest")]
    public class ForgotPasswordRequest : BaseEntity
    {
        [ForeignKey("User")]
        [Required]
        public int UserId { get; set; }

        [Required]
        public string Code { get; set; }

        public virtual UserEntity User { get; set; }
    }
}
