using API.Configurations;
using API.Cores;
using API.Entities;
using API.Security;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using Unity;

namespace API.Services
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
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.Now.AddMinutes(this.JwtSetting.ExpireRefreshToken),
                    CreatedDate = DateTime.Now,
                    CreatedByIP = this.Accessor.HttpContext?.Connection?.RemoteIpAddress?.ToString()
                };
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
