using SqlKata;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using ColumnAttribute = SqlKata.ColumnAttribute;

namespace API.Entities
{
    [Table("refresh_token")]
    public class RefreshTokenEntity : BaseEntity
    {
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("token")]
        public string Token { get; set; }
        [Column("expires")]
        public DateTime Expires { get; set; }
        [Column("created_by_ip")]
        public string CreatedByIP { get; set; }
        [Ignore]
        public bool IsExpired => DateTime.Now >= Expires;
        [Column("revoked")]
        public DateTime? Revoked { get; set; }
        [Column("revoked_by_ip")]
        public string RevokedByIP { get; set; }
        [Ignore]
        public bool IsActive => Revoked == null && !IsExpired;
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
        public RefreshTokenEntity()
        {
            this.CreatedDate = DateTime.Now;
        }
    }
}
