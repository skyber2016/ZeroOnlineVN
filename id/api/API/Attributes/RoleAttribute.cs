using API.Cores.Exceptions;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace API.Attributes
{
    public class RoleAttribute : ActionFilterAttribute
    {
        private IUserService UserService { get; set; }
        public RoleAttribute(IUserService userService)
        {
            this.UserService = userService;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var method = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(true);
            var attrib = method.Any(x => x.GetType() == typeof(AllowAnonymousAttribute));
            if (attrib)
            {
                return;
            }
            var hasRoleAdmin = method.Any(x => x.GetType() == typeof(AdminAttribute));

            if (hasRoleAdmin && !this.UserService.IsAdmin())
            {
                throw new BadRequestException("Bạn không có quyền truy cập");
            }

        }
    }
}
