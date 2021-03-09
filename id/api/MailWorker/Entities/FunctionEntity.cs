using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cambopay_API.Entities
{
    [Table("Function")]
    public class FunctionEntity : BaseEntity
    {
        [StringLength(500)]
        [Required]
        public string Name { get; set; }

        [StringLength(500)]
        [Required]
        public string Code { get; set; }

        [ForeignKey("Parent")]
        public int? ParentId { get; set; }

        [ForeignKey("Page")]
        public int? PageId { get; set; }

        public virtual PageEntity Page { get; set; }
        public virtual FunctionEntity Parent { get; set; }
        public virtual ICollection<FunctionEntity> Childrens { get; set; }
        public virtual ICollection<RoleFunctionEntity> RoleFunctions { get; set; }
    }
}
