using SqlKata;
using System.ComponentModel.DataAnnotations.Schema;
using ColumnAttribute = SqlKata.ColumnAttribute;

namespace API.Entities
{
    [Table("cq_bonus")]
    public class CqBonusEntity : BaseEntity
    {
        [Column("id_account")]
        public int UserId { get; set; }
        [Column("action")]
        public int ActionId { get; set; }
    }
}
