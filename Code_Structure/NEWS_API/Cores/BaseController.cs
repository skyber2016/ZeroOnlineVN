using CORE_API.Cores;
using NEWS_API.Attributes;
using Entity.DTO.Base;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Unity;

namespace NEWS_API.Cores
{
    [TypeFilter(typeof(ErrorHandlingAttribute))]
    public class BaseController : BaseCoreController
    {
        [Dependency]
        public IUnitOfWork UnitOfWork { get; set; }


        protected IActionResult ResponsePagination<T>(int totalRecords, IEnumerable<object> listObj)
        {
            if (listObj == null)
            {
                return Ok(new List<T>());
            }
            var response = listObj.Select(x => Mapper.Map<T>(x));
            return Ok(new PaginationResponse<T>
            {
                TotalRecords = totalRecords,
                Data = response
            });
        }
         
    }
}
