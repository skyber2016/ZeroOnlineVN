using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cambopay_API.Entities
{
    [Table("User")]
    public class UserEntity : BaseEntity, ICloneable
    {
        [Required]
        [StringLength(100)]
        public string Username { get; set; }
        [StringLength(500)]
        [Required]
        public string FullName { get; set; }
        [Required]
        [StringLength(500)]
        public string Password { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string Phone { get; set; }

        [ForeignKey("Bank")]
        [Required]
        public int BankId { get; set; }

        public bool IsChangedPwd { get; set; }
        public virtual BankEntity Bank { get; set; }
        public virtual ICollection<RefreshTokenEntity> RefreshTokens { get; set; }
        public virtual ICollection<UserRoleEntity> UserRoles { get; set; }
        public object Clone()
        {
            return this;
        }
    }
}
