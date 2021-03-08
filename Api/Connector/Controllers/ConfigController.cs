using Forum_API.Cores;
using Forum_API.DTO.SystemConfig.Requests;
using Forum_API.DTO.SystemConfig.Responses;
using Forum_API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Forum_API.Controllers
{
    public class ConfigController : GeneralController<SystemConfigEntity, SystemConfigGetResponse, SystemConfigCreateRequest, SystemConfigCreateResponse, SystemConfigUpdateRequest, SystemConfigUpdateResponse>
    {
        [AllowAnonymous]
        public override async Task<IActionResult> Get(int pageNumber = 1, int pageSize = 10)
        {
            var data = await this.GeneralService.FindBy(x => !x.IsDeleted && x.IsPublic).ToListAsync();
            var resp = data.ToDictionary(x => x.Code, x => x.Value);
            return Response(resp);
        }
    }
}
