

using SqlKata;

namespace API.Entities
{
    public class BaseEntity
    {
        [Column("id")]
        [SqlKata.Key]
        public int? Id { get; set; }
    }
}
