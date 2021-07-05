using System;

namespace NEWS_API.Entities
{
    public static class DateTimeExtensions
    {
        public static string ToddMMyyyy(this DateTime dateTime)
        {
            return $"{dateTime.Day.ToString().PadLeft(2, '0')}/{(dateTime.Month).ToString().PadLeft(2, '0')}/{dateTime.Year}";
        }
        public static string ToddMMyyyy(this DateTime? dt)
        {
            return dt.Value.ToddMMyyyy();
        }
        public static string ToddMMyyyyHHMMss(this DateTime? dt)
        {
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

    }
}
