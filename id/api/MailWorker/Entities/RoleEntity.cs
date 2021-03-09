using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cambopay_API.Entities
{
    [Table("Role")]
    public class RoleEntity : BaseEntity
    {
        [StringLength(500)]
        [Required]
        public string Name { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        [StringLength(500)]
        [Required]
        public int Prioritize { get; set; }

        public virtual ICollection<UserRoleEntity> UserRoles { get; set; }
        public virtual ICollection<RoleFunctionEntity> RoleFunctions { get; set; }

    }
}
