using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity;

namespace CORE_API.Cores
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BaseCoreController : ControllerBase
    {
        [Dependency]
        public IMapper Mapper { get; set; }

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
        protected new IActionResult Response(byte[] fileContents)
        {
            if (fileContents == null)
            {
                return Ok();
            }
            return Ok(Convert.ToBase64String(fileContents));
        }


    }
}
