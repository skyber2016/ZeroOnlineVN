using Forum_API.Configurations;
using Forum_API.Cores;
using Forum_API.Entities;
using Forum_API.Security;
using Forum_API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Unity;

namespace Forum_API.Services
{
    public class JwtService : IJwtService
    {
        [Dependency]
        public IUnitOfWork _unitOfWork { get; set; }

        [Dependency]
        public IHttpContextAccessor Accessor { get; set; }

        private JwtSetting JwtSetting
        {
            get
            {
                return _unitOfWork.JwtSetting.Value;
            }
        }
        public string GenerateJwtToken(UserPrincipal user)
        {
            var claims = user.GetType().GetProperties().Where(x=>x.GetValue(user) != null).Select(x => new Claim(x.Name, x.GetValue(user)?.ToString())).ToArray();
            var builder = new JwtTokenBuilder()
                             .AddSecurityKey(this.GetSymmetricSecurityKey())
                             .AddSubject("Security.Bearer")
                             .AddIssuer("Security.Bearer")
                             .AddAudience("Security.Bearer")
                             .AddExpiry(JwtSetting.Expire)
                             .AddClaims(claims.ToDictionary(x=>x.Type, x=>x.Value))
                             .Build()
                             ;
            return builder.Value;

        }
        public IEnumerable<Claim> GetTokenClaims(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Given token is null or empty.");

            TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters();

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            ClaimsPrincipal tokenValid = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
            return tokenValid.Claims;
        }
        public bool IsTokenValid(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Given token is null or empty.");

            TokenValidationParameters tokenValidationParameters = GetTokenValidationParameters();

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Generate random refresh token
        /// </summary>
        /// <returns></returns>
        public RefreshTokenEntity GenerateRefreshToken()
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshTokenEntity
                {
                    Token = this.CreateMD5(randomBytes),
                    Expires = DateTime.Now.AddMinutes(this.JwtSetting.ExpireRefreshToken),
                    CreatedDate = DateTime.Now,
                    CreatedByIP = this.Accessor.HttpContext?.Connection?.RemoteIpAddress?.ToString()
                };
            }
        }
        private string CreateMD5(byte[] inputBytes)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
        private TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = GetSymmetricSecurityKey(),

            };
        }
        private SecurityKey GetSymmetricSecurityKey()
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(_unitOfWork.JwtSetting.Value.SecretKey);
            var base64Encode = Convert.ToBase64String(plainTextBytes);
            byte[] symmetricKey = Convert.FromBase64String(base64Encode);
            return new SymmetricSecurityKey(symmetricKey);
        }

    }

}
