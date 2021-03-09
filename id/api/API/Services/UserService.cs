using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using Unity;
using API.Security;
using API.Services.Interfaces;

namespace API.Services
{
    public class UserService: IUserService
    {
        [Dependency]
        public IHttpContextAccessor Accessor { get; set; }

        public UserPrincipal GetCurrentUser()
        {
            var user = Accessor.HttpContext.User;
            if(user == null || !user.Claims.Any())
            {
                return null;
            }
            return new UserPrincipal
            {
                Email = user.FindFirst(nameof(UserPrincipal.Email))?.Value,
                Id = Convert.ToInt32(user.FindFirst(nameof(UserPrincipal.Id))?.Value ?? "-1"),
                FullName = user.FindFirst(nameof(UserPrincipal.FullName))?.Value,
                Username = user.FindFirst(nameof(UserPrincipal.Username))?.Value,
            };
        }
        

    }
}
