using AutoMapper;
using Forum_API.Attributes;
using Forum_API.Helpers;
using Forum_API.Security;
using Forum_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Unity;

namespace Forum_API.Cores
{
    [TypeFilter(typeof(ErrorHandlingAttribute))]
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    [TypeFilter(typeof(SingleUserAttribute))]
    [TypeFilter(typeof(RolePermissionAttribute))]
    public class BaseController : ControllerBase
    {
        [Dependency]
        public IMapper Mapper { get; set; }
        [Dependency]
        public IUnitOfWork UnitOfWork { get; set; }
        [Dependency]
        public IUserService UserService { get; set; }
        [Dependency]
        public ILoggerManager Logger { get; set; }
        protected new IActionResult Response()
        {
            return Ok();
        }
        protected new IActionResult Response<TEntity>(object o) where TEntity:class,new()
        {
            if(o == null)
            {
                return Ok(new TEntity());
            }
            var response = Mapper.Map<TEntity>(o);
            return Ok(response);
        }

        protected new IActionResult Response<T>(IEnumerable<object> listObj) where T : class, new()
        {
            if(listObj == null)
            {
                return Ok(new List<T>());
            }
            var response = listObj.Select(x => Mapper.Map<T>(x));
            return Ok(response);
        }
        
        protected new IActionResult Response(object o)
        {
            if(o == null)
            {
                return Ok();
            }
            return Ok(o);
        }
    }
}
