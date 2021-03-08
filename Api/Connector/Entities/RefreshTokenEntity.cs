using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum_API.Entities
{
    [Table("RefreshToken")]
    public class RefreshTokenEntity : BaseEntity
    {
        [ForeignKey("User")]
        public long UserId { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public string CreatedByIP { get; set; }

        public bool IsExpired => DateTime.Now >= Expires;
        public DateTime? Revoked { get; set; }
        public string RevokedByIP { get; set; }

        public bool IsActive => Revoked == null && !IsExpired;


        public virtual UserEntity User { get; set; }
    }
}
