using API.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("cq_user")]
    public class UserEntity : BaseEntity
    {
        public string Name { get; set; }
        public int Emoney { get; set; }
    }
}
