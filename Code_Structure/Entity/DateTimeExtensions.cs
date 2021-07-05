using System;

namespace Entity
{
    public static class DateTimeExtensions
    {
        public static string ToddMMyyyy(this DateTime dateTime)
        {
            return $"{dateTime.Day.ToString().PadLeft(2, '0')}/{(dateTime.Month).ToString().PadLeft(2, '0')}/{dateTime.Year}";
        }
        public static string ToddMMyyyy(this DateTime? dt)
        {
            if (dt == null)
            {
                return null;
            }
            return dt.Value.ToddMMyyyy();
        }
        public static string ToddMMyyyyHHMMss(this DateTime? dt)
        {
            if(dt == null)
            {
                return null;
            }
            return dt.Value.ToddMMyyyyHHMMss();
        }
        public static string ToddMMyyyyHHMMss(this DateTime dt)
        {
            return $"{dt.ToddMMyyyy()} {dt.ToHHMMss()}";
        }
        public static string ToddMMyyyyHHMMss12(this DateTime dt)
        {
            return $"{dt.ToddMMyyyy()} {dt.ToString("hh:mm:ss")}";
        }
        public static string ToHHMMss(this DateTime dt)
        {
            return dt.ToString("HH:mm:ss");
        }
        public static string ToHHMMss(this DateTime? dt)
        {
            return dt.Value.ToHHMMss();
        }

        public static string ToYYYYMMDD(this DateTime dateTime)
        {
            return $"{dateTime.Year}-{(dateTime.Month).ToString().PadLeft(2, '0')}-{dateTime.Day.ToString().PadLeft(2, '0')}";
        }
        public static DateTime StringFormatToDate(this string input)
        {
            var _year = Convert.ToInt32(input.Substring(0, 4));
            var _month = Convert.ToInt32(input.Substring(4, 2));
            var _day = Convert.ToInt32(input.Substring(6, 2));
            var _hour = Convert.ToInt32(input.Substring(8, 2));
            var _minute = Convert.ToInt32(input.Substring(10, 2));
            var _second = Convert.ToInt32(input.Substring(12, 2));

            return new DateTime(_year, _month, _day, _hour, _minute, _second);
        }

        /// <summary>
        /// date format: dd/MM/yyyy => DateTime
        /// </summary>
        /// <param name="dateString">Phai co kieu: dd/MM/yyyy</param>
        /// <returns></returns>
        public static DateTime ToDate(this string dateString)
        {
            if (dateString == null)
            {
                return DateTime.Now;
            }
            if (dateString.Split('/').Length != 3)
            {
                return DateTime.Now;
            }
            var data = dateString.Split('/');
            var day = Convert.ToInt32(data[0]);
            var month = Convert.ToInt32(data[1]);
            var year = Convert.ToInt32(data[2]);
            return new DateTime(year, month, day);
        }
    }
}
