using Forum_API.Security;

namespace Forum_API.Services.Interfaces
{
    public interface IUserService
    {
        UserPrincipal GetCurrentUser();
    }
}
