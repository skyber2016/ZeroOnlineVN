using System.ComponentModel.DataAnnotations.Schema;

namespace Forum_API.Entities
{
    [Table("RoleFunction")]
    public class RoleFunctionEntity : BaseEntity
    {
        [ForeignKey("Role")]
        public long RoleId { get; set; }
        [ForeignKey("Function")]
        public long FunctionId { get; set; }

        public virtual RoleEntity Role { get; set; }
        public virtual FunctionEntity Function { get; set; }
    }
}
