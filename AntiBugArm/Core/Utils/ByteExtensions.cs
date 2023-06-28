using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
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
        public static byte[] Take(this byte[] source, int length)
        {
            var s = source.ToList();
            for (int i = s.Count; i < length; i++)
            {
                s.Add(0x0);
            }
            return s.ToArray();
        }
        public static string vnJoin(this byte[] b)
        {
            return string.Join(" ", b);
        }
        public static byte[] Replace(this byte[] source, byte[] rep, byte[] des)
        {
            var s = source.vnJoin();
            return s.Replace(rep.vnJoin(), des.Take(rep.Length).vnJoin()).StringToByte();
        
        
        }
        public static bool Includes(this byte[] source, byte[] des)
        {
            var s = source.vnJoin();
            var d = des.vnJoin();
            return s.Contains(d);
        }
    }
}
