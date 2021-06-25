using API.Security;

namespace API.Services.Interfaces
{
    public interface IUserService
    {
        UserPrincipal GetCurrentUser();
        bool IsAdmin(string username = null);
    }
}
