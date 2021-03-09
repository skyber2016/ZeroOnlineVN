using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cambopay_API.Entities
{
    [Table("UserRole")]
    public class UserRoleEntity : BaseEntity
    {
        [ForeignKey("User")]
        [Required]
        public int UserId { get; set; }

        [ForeignKey("Role")]
        [Required]
        public int RoleId { get; set; }

        public virtual UserEntity User { get; set; }
        public virtual RoleEntity Role { get; set; }
    }
}
