using Isopoh.Cryptography.Argon2;
using System;
using System.Security.Cryptography;
using System.Text;

namespace CORE_API.Helpers
{
    public static class PasswordHelper
    {
        public static byte[] CreateSalt()
        {
            var buffer = new byte[16];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buffer);
            return buffer;
        }
        public static string HashPassword(string password)
        {
            var hash = Argon2.Hash(password, timeCost: 3, memoryCost: 32768, parallelism: 1, hashLength: 32);
            return hash;
        }
        public static string ToBase64PasswordString(this string password)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(HashPassword(password)));
        }
        public static string ToPasswordString(this string password)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(HashPassword(password)));
        }
        public static string ToSaltString(byte[] salt)
        {
            return Convert.ToBase64String(salt);
        }
    }
}
