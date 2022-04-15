using System;
using System.Security.Cryptography;
using System.Text;

namespace AutoZero.Helpers
{
    public static class HashHelper
    {
        public static string GetSalt()
        {
            int length = 50;
            byte[] randomArray = new byte[length];
            string randomString;
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomArray);
            randomString = Convert.ToBase64String(randomArray);
            return randomString;
        }
        public static string EncryptSHA256Managed(string input)
        {
            UnicodeEncoding uEncode = new UnicodeEncoding();
            byte[] bytClearString = uEncode.GetBytes(input);
            SHA256Managed sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(bytClearString);
            string hex = BitConverter.ToString(hash);
            hex = hex.Replace("-", "");
            return hex;
        }

        public static string Base64Encode(this string str)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
        }
        public static string Base64Decode(this string base64)
        {
            try
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(base64));
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
