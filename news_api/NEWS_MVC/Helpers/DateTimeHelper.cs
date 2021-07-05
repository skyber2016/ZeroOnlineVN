using System;
using System.Security.Cryptography;
using System.Text;

namespace NEWS_MVC.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime StringFormatToDate(this string input)
        {
            var _year = Convert.ToInt32(input.Substring(0, 4));
            var _month = Convert.ToInt32(input.Substring(4, 2));
            var _day = Convert.ToInt32(input.Substring(6, 2));
            var _hour = Convert.ToInt32(input.Substring(8, 2));
            var _minute = Convert.ToInt32(input.Substring(10, 2));
            var _second = Convert.ToInt32(input.Substring(12, 2));

            return new DateTime( _year, _month,_day,_hour,_minute,_second);
        } 
    }
}
