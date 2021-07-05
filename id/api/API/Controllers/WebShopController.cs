using API.Common;
using API.Cores;
using API.Cores.Exceptions;
using API.DTO.WebShop.Requests;
using API.DTO.WebShop.Responses;
using API.Entities;
using API.Helpers;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SqlKata.Execution;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace API.Controllers
{
    public class WebShopController : GeneralController<WebShopEntity>
    {
        [Dependency]
        public IGeneralService<WebShopItemEntity> ShopItemService { get; set; }
        [Dependency]
        public IGeneralService<AccountEntity> AccountService { get; set; }
        [Dependency]
        public IGeneralService<CqBonusEntity> BonusService { get; set; }
        [Dependency]
        public IGeneralService<UserEntity> UserEntService { get; set; }
        [Dependency]
        public IGeneralService<BotMessageEntity> BotService { get; set; }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var logs = await this.GeneralService.FindBy().OrderByDesc("id").Limit(50).GetAsync<WebShopEntity>();
            var response = logs.Select(x => {
                var map = Mapper.Map<WebShopGetResponse>(x);
                map.CharName = map.CharName.Base64Decode();
                map.ItemName = map.ItemName.Base64Decode();
                return map;
            });
            return Response(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(WebShopCreateRequest request)
        {
            var currentUser = this.UserService.GetCurrentUser();
            var item = await this.ShopItemService.SingleBy(new
            {
                id = request.ItemId
            });
            if(item == null)
            {
                throw new BadRequestException("Vật phẩm không tồn tại");
            }
            if (item.Qty <= 0)
            {
                throw new BadRequestException("Vật phẩm này đã hết, hãy quay lại sau");
            }
            var acc = await this.AccountService.SingleBy(new
            {
                id = currentUser.Id
            });
            if(acc == null)
            {
                throw new BadRequestException("Không tìm thấy tài khoản");
            }
            if(acc.WebMoney < item.Price)
            {
                throw new BadRequestException("Tài khoản của bạn không đủ, vui lòng nạp thêm");
            }
            var user = await this.UserEntService.SingleBy(new
            {
                account_id = currentUser.Id
            });
            if(user == null)
            {
                throw new BadRequestException("Vui lòng tạo nhân vật trước");
            }

            acc.WebMoney -= item.Price;
            acc.CheckSum = acc.GetCheckSum();

            await this.BonusService.AddAsync(new CqBonusEntity
            {
                ActionId = item.Id.Value,
                UserId = currentUser.Id
            });

            item.Qty -= 1;
            await ShopItemService.UpdateAsync(item);
            await GeneralService.AddAsync(new WebShopEntity
            {
                ItemId = item.Id.Value,
                Price = item.Price,
                UserId = currentUser.Id,
                ItemName = item.Name,
                CharName = user.Name.Base64Encode()
            });
            await AccountService.UpdateAsync(acc);
            var builder = new StringBuilder();
            builder.AppendLine("-------- **WebShop** --------");
            builder.AppendLine($"**Tài khoản:** {currentUser.Username}");
            builder.AppendLine($"**Vật phẩm:** {item.Name.Base64Decode()}");
            builder.AppendLine($"**Giá:** {item.Price}");
            builder.AppendLine($"**Tiền còn lại:** {acc.WebMoney}");
            builder.AppendLine($"**Thời gian:** {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
            await this.BotService.AddAsync(new BotMessageEntity
            {
                Channel = ChannelConstant.WEB_SHOP.ToString(),
                Message = builder.ToString().Base64Encode()
            });
            return Response();
        }
    }
}
