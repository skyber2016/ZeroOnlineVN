using AutoMapper;
using CORE_API.Cores;
using Microsoft.AspNetCore.Mvc;
using NEWS_MVC.Attributes;
using Unity;

namespace NEWS_MVC.Cores
{
    [TypeFilter(typeof(ErrorHandlingAttribute))]
    public class BaseController : Controller
    {
        [Dependency]
        public IUnitOfWork UnitOfWork { get; set; }
        [Dependency]
        public IMapper Mapper { get; set; }
         
    }
}
