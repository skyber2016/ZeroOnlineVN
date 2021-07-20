using System;

namespace API.DTO.User.Responses
{
    public class UserGetMoneyResponse
    {
        private int _money { get; set; }
        public string WebMoney
        {
            get
            {
                return this._money.ToString("#,##0");
            }
            set
            {
                this._money = Convert.ToInt32(value);
            }
        }

        public int Vip { get; set; }
        public string Current { get; set; }
        public string Total { get; set; }
        public string Sub { get; set; }
        public string Percent { get; set; }
    }
}
