using SqlKata;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using ColumnAttribute = SqlKata.ColumnAttribute;

namespace API.Entities
{
    [Table("card")]
    public class CardEntity : BaseEntity
    {
        [Column("type")]
        public int Type { get; set; }
        [Column("seri")]
        public string Seri { get; set; }
        [Column("code")]
        public string Code { get; set; }
        [Column("value")]
        public int Value { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("trans_id")]
        public string TranId { get; set; }
        [Column("message")]
        public string Message { get; set; }
        [Column("amount")]
        public string Amount { get; set; }
        [Column("created_date")]
        public DateTime CreatedDate { get; set; }
        [Column("account_id")]
        public int AccountId { get; set; }

        public CardEntity()
        {
            string trans_id = (Convert.ToInt64(DateTime.Now.ToString("yyMMddHHmmss"))).ToString("X");
            this.TranId = trans_id;
            this.Message = "Thẻ nạp chậm, vui lòng chờ Admin check card";
            this.CreatedDate = DateTime.Now;
        }
    }
}
