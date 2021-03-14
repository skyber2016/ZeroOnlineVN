using API.Configurations;
using API.Cores.Exceptions;
using API.Entities;
using API.Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Unity;

namespace API.Services
{
    public class CardChargeResponse
    {
        public int code { get; set; }
        public string amount { get; set; }
        public string msg { get; set; }
        public string trans_id { get; set; }

    }
    public class CardService : GeneralService<CardEntity>, ICardService
    {
        [Dependency]
        public IOptions<NapTheNgaySetting> Options { get; set; }
        public async Task<CardEntity> CardCharge(CardEntity card)
        {
            var rand = new Random();
            int card_id = card.Type; //Nha mang ung voi the ma KH gui len. Viettel->1, Vinaphone->3, MObifone->2
            int cardValue = card.Value; //Menh gia cua the nap
            string pin_field = card.Code; //Ma the
            string seri_field = card.Seri; //Serial the
            var cardResult = await CardCharge(card_id, pin_field, seri_field, card.TranId, cardValue);
            card.Status = cardResult.code;
            card.Message = cardResult.msg;
            card.Amount = cardResult.amount;
            
            return card;
        }


        private async Task<CardChargeResponse> CardCharge(int telco, string mathe, string seri, string transid, int cardValue)
        {
            string urlWS = "http://api.napthengay.com/v2/";
            string merchantId = this.Options.Value.MerchantId.ToString(); //Tham so lay tu website napthengay.com
            string Secretkey = this.Options.Value.Secretkey; //Tham so lay tu website napthengay.com
            string apiMail = this.Options.Value.Email;//Dia chi mail dang ky tai khoan tren napthengay.com
            var plaintText = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", merchantId, apiMail, transid, telco, cardValue, mathe, seri, "md5", Secretkey);
            string key = GetMD5Hash(plaintText);
            using (var client = new HttpClient())
            {
                var dic = new Dictionary<string, string>
                {
                    ["merchant_id"] = merchantId,
                    ["card_id"] = telco.ToString(),
                    ["seri_field"] = seri,
                    ["pin_field"] = mathe,
                    ["trans_id"] = transid,
                    ["data_sign"] = key,
                    ["algo_mode"] = "md5",
                    ["api_email"] = apiMail,
                    ["card_value"] = cardValue.ToString(),
                };
                var result = await client.PostAsync(urlWS, new FormUrlEncodedContent(dic));
                if (!result.IsSuccessStatusCode)
                {
                    throw new BadRequestException("Xảy ra lỗi không xác định");
                }
                var content = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CardChargeResponse>(content);
            }
        }
        private static string GetMD5Hash(string input)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string password = s.ToString();
            return password;
        }
    }
}
