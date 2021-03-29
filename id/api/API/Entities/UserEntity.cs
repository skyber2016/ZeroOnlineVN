using SqlKata;
using System.ComponentModel.DataAnnotations.Schema;
using ColumnAttribute = SqlKata.ColumnAttribute;

namespace API.Entities
{
    [Table("cq_user")]
    public class UserEntity : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; }

        [Column("Battle_lev")]
        public int BattleLev { get; set; }

        [Column("donation")]
        public long Donation { get; set; }

        [Column("VIP")]
        public long Vip { get; set; }
    }
}
