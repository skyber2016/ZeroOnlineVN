

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace NEWS_API.Helpers
{
    public static class CommonHelper
    {
        public static string toUrlFriendly(string text)
        {
            string stFormD = text.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            var str = (sb.ToString().Normalize(NormalizationForm.FormD));
            str = str.Replace(" ", "-");
            while (str.Contains("--") || str.Contains(",-"))
            {
                str = str.Replace("--", "-");
                str = str.Replace(",-", ",");
            }
            return str;
        }
        public static string ConvertBirthDayExim(string birth)
        {
            try
            {
                if (!string.IsNullOrEmpty(birth))
                {
                    string ngay = "";
                    string thang = "";
                    string nam = "";
                    nam = birth.Substring(0, 4);
                    thang = birth.Substring(4, 2);
                    ngay = birth.Substring(6, 2);
                    return ngay + "/" + thang + "/" + nam;
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string ConvertDdMmYyToMmDdYy(this string strFdate) //= "ddMMyyyy"
        {
            string[] tem = strFdate.Split('/');
            return tem[1] + "/" + tem[0] + "/" + tem[2];
        }
        public static DateTime? DateTimeToDateSearch(string s)
        {
            DateTime date;
            if (DateTime.TryParse(s, out date)) return date.AddHours(23).AddMinutes(59).AddSeconds(59);
            return null;
        }
        public static DateTime? ToDateTimeNullable(string s)
        {
            DateTime date;
            if (DateTime.TryParse(s, out date)) return date;
            return null;
        }
        public static string SetPassword()
        {
            return RandomString(5) + RandomStringUpper(1) + RandomStringNumber(1) + RandomStringSpeciel(1);
        }
        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHJKMNPQRSTUVWXYZabcdefghjkmnopqrstuvwxyz123456789@#$%&";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string RandomStringUpper(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHJKMNPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string RandomStringNumber(int length)
        {
            Random random = new Random();
            const string chars = "123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string RandomStringSpeciel(int length)
        {
            Random random = new Random();
            const string chars = "@#$%&";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static JObject ConvertObjectToJson(object o)
        {
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            var reqObject = System.Text.Json.JsonSerializer.Serialize(o, serializeOptions);
            return JObject.Parse(reqObject);
        }
    }
}
