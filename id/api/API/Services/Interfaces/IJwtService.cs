using API.Entities;
using API.Security;
using System.Collections.Generic;
using System.Security.Claims;

namespace API.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(UserPrincipal user);
        IEnumerable<Claim> GetTokenClaims(string token);
        bool IsTokenValid(string token);
        RefreshTokenEntity GenerateRefreshToken();
    }
}
