using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiddlewareTCP
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
        public static string Split(this byte[] b)
        {
            return string.Join(" ", b);
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

        public static bool FindPattern(this byte[] source, string[] des) 
        {
            if(source.Length != des.Length)
            {
                return false;
            }
            for (int i = 0; i < des.Length; i++)
            {
                var sourceIndex = source[i];
                var desIndex = des[i];
                if (desIndex == "?") continue;
                var desByte = Convert.ToInt32(desIndex);
                if(sourceIndex != desByte)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
