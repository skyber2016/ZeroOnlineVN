using SqlKata;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using ColumnAttribute = SqlKata.ColumnAttribute;

namespace API.Entities
{
    [Table("web_reward_vip")]
    public class RewardVipEntity : BaseEntity
    {
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("vip")]
        public int Vip { get; set; }
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
    }
}
