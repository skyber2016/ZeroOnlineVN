using Forum_API.Attributes;
using Forum_API.Cores.Exceptions;
using Forum_API.Entities;
using Forum_API.Helpers;
using Forum_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using Unity;

namespace Forum_API.Security
{
    public class RolePermissionAttribute : ActionFilterAttribute
    {
        private IUserService UserPrincial { get; set; }
        private IGeneralService<RoleEntity> RoleService { get; set; }
        private IGeneralService<FunctionEntity> FunctionService { get; set; }
        private IGeneralService<RoleFunctionEntity> RoleFunctionService { get; set; }
        private ILoggerManager Logger { get; set; }
        public RolePermissionAttribute(
            IUserService userPrincial, 
            IGeneralService<RoleEntity> roleService,
            IGeneralService<FunctionEntity> functionService,
            ILoggerManager logger,
            IGeneralService<RoleFunctionEntity> roleFunctionService
            )
        {
            this.FunctionService = functionService;
            this.UserPrincial = userPrincial;
            this.Logger = logger;
            this.RoleService = roleService;
            this.RoleFunctionService = roleFunctionService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var attrib = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(true).Any(x => x.GetType() == typeof(AllowAnonymousAttribute));
            if (attrib)
            {
                return;
            }
            var ignore = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(true).Any(x => x.GetType() == typeof(IgnoreRoleAttribute));
            if (ignore)
            {
                return;
            }
            var actionContext = context.ActionDescriptor as ControllerActionDescriptor;
            var controllerName = actionContext.ControllerName;
            var actionName = actionContext.ActionName;
            var currentUser = this.UserPrincial.GetCurrentUser();
            if (currentUser.IsSystem)
            {
                return;
            }
            var hasPermission = this.RoleFunctionService.FindBy(x => x.RoleId == currentUser.RoleId).Select(x=>x.Function).Where(x=>x.Code == $"{controllerName}_{actionName}").Any();
            if (hasPermission)
            {
                return;
            }
            throw new ForbidenException();
        }
    }
}
