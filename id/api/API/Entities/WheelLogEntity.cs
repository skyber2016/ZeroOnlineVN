using SqlKata;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using ColumnAttribute = SqlKata.ColumnAttribute;

namespace API.Entities
{
    [Table("wheel_log")]
    public class WheelLogEntity : BaseEntity
    {
        [Column("desc")]
        public string Desc { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("account_id")]
        public int AccountId { get; set; }
    }
}
