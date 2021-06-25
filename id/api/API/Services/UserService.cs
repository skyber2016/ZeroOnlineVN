using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using Unity;
using API.Security;
using API.Services.Interfaces;
using API.Entities;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Extensions.Options;
using API.Configurations;

namespace API.Services
{
    public class UserService: IUserService
    {
        [Dependency]
        public IHttpContextAccessor Accessor { get; set; }

        [Dependency]
        public IOptions<AppSettings> Config { get; set; }

        private UserPrincipal User { get; set; }

        public UserPrincipal GetCurrentUser()
        {
            var user = Accessor.HttpContext.User;
            if(user == null || !user.Claims.Any())
            {
                return null;
            }
            if(this.User == null)
            {
                this.User = new UserPrincipal
                {
                    Email = user.FindFirst(nameof(UserPrincipal.Email))?.Value,
                    Id = Convert.ToInt32(user.FindFirst(nameof(UserPrincipal.Id))?.Value ?? "-1"),
                    FullName = user.FindFirst(nameof(UserPrincipal.FullName))?.Value,
                    Username = user.FindFirst(nameof(UserPrincipal.Username))?.Value
                };
            }
            return this.User;
        }

        public bool IsAdmin(string username = null)
        {
            try
            {
                var currentUser = username;
                if(username == null)
                {
                    currentUser = this.GetCurrentUser()?.Username;
                }
                return this.Config.Value.Admin.Contains(currentUser);
            }
            catch (Exception)
            {
                return false;
            }
            
        }
        
    }
}
