using SqlKata;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using ColumnAttribute = SqlKata.ColumnAttribute;

namespace API.Entities
{
    [Table("web_logs")]
    public class WebLogEntity : BaseEntity
    {
        [Column("message")]
        public string Message { get; set; }
        [Column("type")]
        public int Type { get; set; }
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
        [Column("account_id")]
        public int AccountId { get; set; }
        [Column("value")]
        public long Value { get; set; }
        public WebLogEntity()
        {
            this.CreatedDate = DateTime.Now;
        }
    }
}
