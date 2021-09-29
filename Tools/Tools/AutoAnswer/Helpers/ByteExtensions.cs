using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoAnswer
{
    public static class ByteExtensions
    {
        public static byte[] ToByte(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }
        public static string ConvertToString(this byte[] str)
        {
            return Encoding.UTF8.GetString(str);
        }
        public static byte[] ToByte(this int Integer)
        {
            return BitConverter.GetBytes(Integer);
        }
        public static byte[] StringToByte(this string str)
        {
            return str.Split(' ').Select(x => Convert.ToByte(x)).ToArray();
        }
        public static byte[] HexToByte(this string str)
        {
            return str.Split(' ').Select(x => Convert.ToInt32("0x" + x, 16)).Select(x=> Convert.ToByte(x)).ToArray();
        }
        public static byte[] Take(this byte[] source, int length)
        {
            var s = source.ToList();
            for (int i = s.Count; i < length; i++)
            {
                s.Add(0x0);
            }
            return s.ToArray();
        }
        public static string Split(this byte[] b)
        {
            return b.vnToString();
        }
        public static byte[] Replace(this byte[] source, byte[] rep, byte[] des)
        {
            var s = source.Split();
            return s.Replace(rep.Split(), des.Take(rep.Length).Split()).StringToByte();
        
        
        }
        public static bool Includes(this byte[] source, byte[] des)
        {
            var s = source.Split();
            var d = des.Split();
            return s.Contains(d);
        }
        public static string vnToString(this byte[] source)
        {
            return string.Join(" ", source);
        }
        public static string vnToStringHex(this byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2} ", b);
            return hex.ToString().ToUpper().Trim();
        }

        public static byte[] vnClone(this byte[] source)
        {
            byte[] dest = source.ToArray();
            return dest;
        }

        public static string Join<T>(this IEnumerable<T> source, string sepa = " ")
        {
            return string.Join(sepa, source);
        }
        
        public static string GetString(this byte[] src)
        {
            return Encoding.UTF8.GetString(src);
        }
        public static int GetNumber(this byte[] src)
        {
            var dest = src.FirstOrDefault(x => x != 0);
            return Convert.ToInt32(dest);
        }

        public static int GetNumber(this string src)
        {
            src = src.Replace("\0", "");
            var partern = "0123456789";
            var result = "";
            foreach (var item in src)
            {
                if(partern.Contains(item))
                {
                    result += item;
                }
            }
            return Convert.ToInt32(result);
        }
    }
}
