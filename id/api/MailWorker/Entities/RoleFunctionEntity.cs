using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cambopay_API.Entities
{
    [Table("RoleFunction")]
    public class RoleFunctionEntity : BaseEntity
    {
        [ForeignKey("Role")]
        [Required]
        public int RoleId { get; set; }
        [ForeignKey("Function")]
        [Required]
        public int FunctionId { get; set; }

        public virtual RoleEntity Role { get; set; }
        public virtual FunctionEntity Function { get; set; }
    }
}
