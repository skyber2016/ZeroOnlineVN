using Forum_API.Security;
using Forum_API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using Unity;

namespace Forum_API.Services
{
    public class UserService : IUserService
    {
        public UserPrincipal UserPrincipal { get; set; }
        [Dependency]
        public IHttpContextAccessor Accessor { get; set; }
        public UserPrincipal GetCurrentUser()
        {
            if(this.UserPrincipal != null)
            {
                return this.UserPrincipal;
            }
            var user = this.Accessor.HttpContext.User;
            if(user == null || !user.Claims.Any())
            {
                return null;
            }
            return new UserPrincipal
            {
                Email = user.FindFirst(nameof(UserPrincipal.Email))?.Value,
                Id = Convert.ToInt64(user.FindFirst("Id").Value),
                FullName = user.FindFirst(nameof(UserPrincipal.FullName))?.Value,
                Username = user.FindFirst(nameof(UserPrincipal.Username))?.Value,
                RoleId = Convert.ToInt64(user.FindFirst("RoleId").Value),
                Prioritize = Convert.ToInt32(user.FindFirst(nameof(UserPrincipal.Prioritize))?.Value ?? "0"),
                RoleColor = user.FindFirst(nameof(UserPrincipal.RoleColor))?.Value,
                Avatar = user.FindFirst(nameof(UserPrincipal.Avatar))?.Value
            };
        }
        

    }
}
