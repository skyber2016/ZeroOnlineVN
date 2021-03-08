using Forum_API.Common;
using Forum_API.Cores;
using Forum_API.Cores.Exceptions;
using Forum_API.Cores.WebSockets.Handler;
using Forum_API.DTO.Base;
using Forum_API.DTO.ShoutBox.Requests;
using Forum_API.DTO.ShoutBox.Responses;
using Forum_API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using Unity;

namespace Forum_API.Controllers
{
    public class ShoutBoxController : GeneralController<ShoutBoxEntity, ShoutBoxGetResponse, ShoutBoxCreateRequest, ShoutBoxCreateResponse, ShoutBoxUpdateRequest, ShoutBoxUpdateResponse>
    {
        [Dependency]
        public ShoutBoxHandler ShoutBoxHandler { get; set; }
        public override async Task<IActionResult> Create(ShoutBoxCreateRequest request)
        {
            var entity = Mapper.Map<ShoutBoxEntity>(request);
            entity = await this.GeneralService.AddAsync(entity);
            var dataInserted = await this.GeneralService.FindBy(x => x.Id == entity.Id).Include(x=>x.CreatedByUser).FirstOrDefaultAsync();
            var resp = Mapper.Map<ShoutBoxHandlerResponse>(dataInserted);
            resp.Type = 1;
            await this.ShoutBoxHandler.SendMessageToAllAsync(JsonConvert.SerializeObject(resp));
            return Response<ShoutBoxCreateResponse>(dataInserted);
        }

        public override async Task<IActionResult> Delete(UpdateRequest request)
        {
            var entity = this.GeneralService.SingleBy(x => x.Id == request.Id);
            if (entity == null)
            {
                throw new NotFoundException();
            }
            await this.GeneralService.Delete(entity);
            var resp = Mapper.Map<ShoutBoxHandlerResponse>(entity);
            resp.Type = 3;
            await this.ShoutBoxHandler.SendMessageToAllAsync(JsonConvert.SerializeObject(resp));
            return Response<ShoutBoxUpdateResponse>(entity);
        }

        [AllowAnonymous]
        public override async Task<IActionResult> Get(int pageNumber = 1, int pageSize = 10)
        {
            var maxPageSize = 500;
            if (pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }
            var skip = (pageNumber - 1) * pageSize;
            var data = this.GeneralService.FindBy(x => !x.IsDeleted);
            var totalRecords = await data.CountAsync();
            var result = await data.OrderByDescending(x => x.CreatedDate).Skip(skip).Take(pageSize).ToListAsync();
            var baseResponse = new PaginationResponse<ShoutBoxGetResponse>
            {
                TotalRecords = totalRecords,
                Data = result.Select(x => Mapper.Map<ShoutBoxGetResponse>(x))
            };
            return Response(baseResponse);
        }

        public override async Task<IActionResult> Update(ShoutBoxUpdateRequest request)
        {
            var entity = this.GeneralService.SingleBy(x => x.Id == request.Id && !x.IsDeleted);
            if (entity == null)
            {
                throw new NotFoundException();
            }
            entity = Mapper.Map(request, entity);
            entity = await this.GeneralService.UpdateAsync(entity);
            var resp = Mapper.Map<ShoutBoxHandlerResponse>(entity);
            resp.Type = 2;
            await this.ShoutBoxHandler.SendMessageToAllAsync(JsonConvert.SerializeObject(resp));
            return Response<ShoutBoxUpdateResponse>(entity);
        }
    }
}
