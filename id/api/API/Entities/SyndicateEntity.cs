using System.ComponentModel.DataAnnotations.Schema;
using SqlKata;
using ColumnAttribute = SqlKata.ColumnAttribute;

namespace API.Entities
{
    [Table("cq_syndicate")]
    public class SyndicateEntity : BaseEntity
    {
		[Column("name")]
		public string Name { get; set; }
		[Column("leader_name")]
		public string LeaderName { get; set; }
		[Column("amount")]
		public int Amount { get; set; }
		[Column("rank")]
		public int Rank { get; set; }
		[Column("money")]
		public long Money { get; set; }

		public string MoneyStr
        {
            get
            {
				return this.Money.ToString("#,##0");
            }
        }
	}
}
