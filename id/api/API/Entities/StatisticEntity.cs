using SqlKata;
using System.ComponentModel.DataAnnotations.Schema;
using Column = SqlKata.ColumnAttribute;

namespace API.Entities
{
    [Table("cq_statistic")]
    public class StatisticEntity : BaseEntity
    {
        [Column("iduser")]
        public int IdUser { get; set; }
        [Column("event_type")]
        public int EventType { get; set; }
        [Column("DATA")]
        public int Data { get; set; }
        [Column("eventime")]
        public int EventTime { get; set; }
        [Column("ownertype")]
        public int OwnerType { get; set; }
    }
}
