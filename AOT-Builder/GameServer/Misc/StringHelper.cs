using System.Text;

namespace GameServer
{
    public static class StringHelper
    {
        public static byte[] ToByteArray(this string input, int len)
        {
            var dest = new byte[len];
            byte[] source = Encoding.UTF8.GetBytes(input);
            var startAt = dest.Length - dest.Length;
            Array.Copy(source, dest, source.Length);
            return dest;
        }
        public static string CharsToString(this char[] input)
        {
            var dest = string.Empty;
            foreach (char c in input)
            {
                if(c != char.MinValue)
                {
                    dest += c;
                    continue;
                }
                else
                {
                    break;
                }
            }
            return dest;    
        }
    }
}
