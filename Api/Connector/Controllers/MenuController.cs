using Forum_API.Cores;
using Forum_API.DTO.Page.Requests;
using Forum_API.DTO.Page.Responses;
using Forum_API.Entities;
using Forum_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Unity;

namespace Forum_API.Controllers
{
    public class MenuController : GeneralController<MenuEntity, PageGetResponse, PageCreateRequest, PageCreateResponse, PageUpdateRequest, PageUpdateResponse>
    {
        [Dependency]
        public ISystemConfigService SystemConfigService { get; set; }
        [Dependency]
        public IFunctionService FunctionService { get; set; }
        [Dependency]
        public IRoleFunctionService RoleFunctionService { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public override async Task<IActionResult> Get(int pageNumber = 1, int pageSize = 10)
        {
            return await base.Get(pageNumber, pageSize);
        }
    }
}
