using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cambopay_API.Entities
{
    [Table("RefreshToken")]
    public class RefreshTokenEntity : BaseEntity
    {
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public DateTime Expires { get; set; }
        public string CreatedByIP { get; set; }

        public bool IsExpired => DateTime.Now >= Expires;
        public DateTime? Revoked { get; set; }
        public string RevokedByIP { get; set; }

        public bool IsActive => Revoked == null && !IsExpired;

        public virtual UserEntity User { get; set; }
        public RefreshTokenEntity()
        {
            this.CreatedDate = DateTime.Now;
        }
    }
}
