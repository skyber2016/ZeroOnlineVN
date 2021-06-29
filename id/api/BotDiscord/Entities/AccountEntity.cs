using SqlKata;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using ColumnAttribute = SqlKata.ColumnAttribute;

namespace API.Entities
{
    [Table("account")]
    public class AccountEntity : BaseEntity
    {
        [Column("name")]
        public string Username { get; set; }

        [Column("is_admin")]
        public bool IsAdmin { get; set; }

        [Column("VIP")]
        public int VIP { get; set; }

        [Column("netbar_ip")]
        public string NetbarIP { get; set; }

        [Column("ip_mask")]
        public string IPMask { get; set; }

        [Column("webmoney")]
        public int WebMoney { get; set; }

        [Column("Money")]
        public int Money { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("reg_date")]
        public DateTime RegDate { get; set; }

        [Column("Question1")]
        public string Question { get; set; }

        [Column("Answer1")]
        public string Answer { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("sdt")]
        public string Sdt { get; set; }

        [Column("check_sum")]
        public string CheckSum { get; set; }

        public AccountEntity()
        {
            this.RegDate = DateTime.Now;
        }



        public string GetCheckSum()
        {
            var hashCode = "12345678!@#$%^&*()vndc" + this.Username;
            var checkSum = MD5Helper.HashPassword((this.WebMoney.ToString() + hashCode));
            return checkSum;
        }

        public bool MoneyValid()
        {
            return this.CheckSum == this.GetCheckSum();
        }
    }
}
