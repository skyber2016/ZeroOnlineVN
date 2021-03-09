using System;
using System.IdentityModel.Tokens.Jwt;

namespace API.Security
{
    public sealed class JwtToken
    {
        private JwtSecurityToken Token { get; set; }

        internal JwtToken(JwtSecurityToken token)
        {
            this.Token = token;
        }

        public DateTime ValidTo => Token.ValidTo;
        public string Value => new JwtSecurityTokenHandler().WriteToken(this.Token);
    }
}
