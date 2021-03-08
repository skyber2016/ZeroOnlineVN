using Forum_API.DTO.Authenticate.Requests;
using Forum_API.DTO.Authenticate.Responses;
using Forum_API.Entities;
using System.Threading.Tasks;

namespace Forum_API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> Authenticate(string username, string password);
        Task<UserEntity> Register(RegisterRequest request);
        Task<string> RefreshToken(string refreshToken);
        Task RevokeToken(long userId);
    }
}
