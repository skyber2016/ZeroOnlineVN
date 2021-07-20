using API.Attributes;
using API.Cores;
using API.Cores.Exceptions;
using API.DTO.RewardShopItem.Responses;
using API.Entities;
using API.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class RewardShopItemController : GeneralController<RewardShopItemEntity>
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var items = await this.GeneralService.GetAll();
            var response = items.Select(x =>
             {
                 var map = Mapper.Map<RewardShopItemGetResponse>(x);
                 x.Name = x.Name.Base64Decode();
                 return x;
             });
            return Response(response);
        }
        [HttpPost]
        [Admin]
        public async Task<IActionResult> Create(RewardShopItemCreateRequest request)
        {
            var entity = await this.GeneralService.SingleBy(new
            {
                action_id = request.ActionId,
                group = request.Group
            });
            if(entity != null)
            {
                throw new BadRequestException("Vật phẩm này đã tồn tại trong mốc");
            }
            entity = Mapper.Map<RewardShopItemEntity>(request);
            entity.Name = entity.Name.Base64Encode();
            await this.GeneralService.AddAsync(entity);
            return Response();
        }
        [HttpPut]
        [Admin]
        public async Task<IActionResult> Update(RewardShopItemUpdateRequest request)
        {
            var entity = await this.GeneralService.SingleBy(new
            {
                id = request.Id
            });
            if (entity == null)
            {
                throw new BadRequestException("Vật phẩm này không tồn tại trong mốc");
            }
            entity = Mapper.Map(request,entity);
            entity.Name = entity.Name.Base64Encode();
            await this.GeneralService.UpdateAsync(entity);
            return Response();
        }
        [HttpDelete]
        [Admin]
        public async Task<IActionResult> Update(int id)
        {
            var entity = await this.GeneralService.SingleBy(new
            {
                id = id
            });
            if (entity == null)
            {
                throw new BadRequestException("Vật phẩm này không tồn tại trong mốc");
            }
            await this.GeneralService.DeleteAsync(entity);
            return Response();
        }
    }
}
