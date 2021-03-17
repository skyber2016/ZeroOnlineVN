using API.Entities;
using SqlKata;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using ColumnAttribute = SqlKata.ColumnAttribute;

namespace MiddlewareTCP.Entities
{
    [Table("login_history")]
    public class LoginHistoryEntity : BaseEntity
    {
        [Column("username")]
        public string Username { get; set; }

        [Column("login_time")]
        public DateTime LoginTime { get; set; }

        [Column("logout_time")]
        public DateTime? LogoutTime { get; set; }

        [Column("session_id")]
        public string SessionId { get; set; }
    }
}
