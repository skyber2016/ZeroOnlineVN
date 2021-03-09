using API.Security;

namespace API.Services.Interfaces
{
    public interface IUserService
    {
        UserPrincipal GetCurrentUser();
    }
}
