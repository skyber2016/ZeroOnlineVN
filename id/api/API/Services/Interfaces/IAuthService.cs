using API.DTO.Authenticate.Requests;
using API.DTO.Authenticate.Responses;
using API.Entities;
using System.Threading.Tasks;

namespace API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> Authenticate(string username, string password);
        Task<AccountEntity> Register(RegisterRequest request);
        Task<string> RefreshToken(string refreshToken);
        Task RevokeToken(int userId);
    }
}
