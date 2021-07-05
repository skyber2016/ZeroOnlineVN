using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace NEWS_API.Helpers
{
    public static class HashHelper
    {
        public static string MD5(this string input)
        {
            var md5Crypto = new MD5CryptoServiceProvider();
            md5Crypto.ComputeHash(UTF8Encoding.UTF8.GetBytes(input));
            byte[] results = md5Crypto.Hash;
            StringBuilder str = new StringBuilder();
            foreach (var result in results)
            {
                str.Append(result.ToString("x2"));
            }
            return str.ToString().ToUpper();
        }
        public static bool Verify(this string password, string passwordHash)
        {
            return password.MD5().ToLower() == passwordHash.ToLower();
        }

        public static string Generate()
        {
            return RandomString(5) + RandomStringUpper(1) + RandomStringNumber(1) + RandomStringSpecial(1);
        }

        private static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghjkmnopqrstuvwxyz123456789@#$%&";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static string RandomStringUpper(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static string RandomStringNumber(int length)
        {
            Random random = new Random();
            const string chars = "123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static string RandomStringSpecial(int length)
        {
            Random random = new Random();
            const string chars = "@#$%&";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
