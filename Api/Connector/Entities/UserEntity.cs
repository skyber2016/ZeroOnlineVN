using System.ComponentModel.DataAnnotations.Schema;

namespace Forum_API.Entities
{
    [Table("User")]
    public class UserEntity : BaseEntity
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsEnabled { get; set; }
        public string Avatar { get; set; }
        public long Money { get; set; }

        [ForeignKey("Role")]
        public long RoleId { get; set; }
        public virtual RoleEntity Role { get; set; }
    }
}
