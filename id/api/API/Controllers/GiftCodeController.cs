using API.Attributes;
using API.Common;
using API.Cores;
using API.Cores.Exceptions;
using API.DTO.GiftCode.Requests;
using API.Entities;
using API.Helpers;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SqlKata.Execution;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace API.Controllers
{
    public class GiftCodeController : GeneralController<GiftCodeEntity>
    {
        [Dependency]
        public IGeneralService<GiftCodeLogEntity> GiftCodeLogService { get; set; }
        [Dependency]
        public IGeneralService<CqBonusEntity> BonusService { get; set; }
        [Dependency]
        public IGeneralService<BotMessageEntity> BotMessageService { get; set; }
        [Dependency]
        public IGeneralService<UserEntity> CqUserService { get; set; }

        [HttpPost]
        [Route("Receive")]
        public async Task<IActionResult> Receive(GiftCodeReceiveRequest request)
        {
            var giftCode = request.GiftCode;
            var gift = await this.GeneralService.SingleBy(new
            {
                code = giftCode
            });
            if(gift == null)
            {
                throw new BadRequestException("Rất tiếc, gift code này không tồn tại");
            }
            if (gift.IsUsed)
            {
                throw new BadRequestException("Rất tiếc, gift code này đã được sử dụng");
            }
            var currentUser = this.UserService.GetCurrentUser();
            var log = await this.GiftCodeLogService.SingleBy(new
            {
                gift_code_id = gift.Id,
                user_id = currentUser.Id
            });
            if(log != null)
            {
                throw new BadRequestException("Bạn đã nhận Gift Code này trước đó rồi");
            }
            var user = await this.CqUserService.SingleBy(new
            {
                account_id = currentUser.Id
            });
            if(user == null)
            {
                throw new BadRequestException("Vui lòng tạo nhân vật trước");
            }
            var bonus = await this.BonusService.AddAsync(new CqBonusEntity
            {
                ActionId = gift.ItemId,
                UserId = currentUser.Id
            });

            if(gift.Type == GiftCodeTypeConstant.SINGLE)
            {
                gift.IsUsed = true;
                await this.GeneralService.UpdateAsync(gift);
            }
            var newLog = await this.GiftCodeLogService.AddAsync(new GiftCodeLogEntity
            {
                GiftCodeId = gift.Id.Value,
                UserId = currentUser.Id
            });
            var build = new StringBuilder();
            build.AppendLine($"-------- **Nhận Gift Code** --------");
            build.AppendLine($"**Tài khoản**: {currentUser.Username}");
            build.AppendLine($"**Gift Code**: {gift.Code}");
            var type = gift.Type == GiftCodeTypeConstant.SINGLE ? "1 cho 1" : "1 cho nhiều";
            build.AppendLine($"**Loại**: { type }");
            build.AppendLine($"**Action ID**: { gift.ItemId }");
            build.AppendLine($"**Thời gian**: { newLog.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss") }");
            await this.BotMessageService.AddAsync(new BotMessageEntity
            {
                Channel = ChannelConstant.GIFT_CODE.ToString(),
                Message = build.ToString().Base64Encode()
            });
            return Response();
        }

        [HttpPost]
        [Admin]
        public async Task<IActionResult> Create(GiftCodeCreateRequest request)
        {
            var giftCode = request.GiftCode;
            var gift = await this.GeneralService.SingleBy(new
            {
                code = giftCode
            });
            if(gift != null)
            {
                throw new BadRequestException("Gift code đã tồn tại");
            }

            await this.GeneralService.AddAsync(new GiftCodeEntity
            {
                Code = request.GiftCode,
                ItemId = request.ItemId,
                Type = request.Type
            });
            
            return Response();
        }

        [HttpGet]
        [Admin]
        public async Task<IActionResult> Get()
        {
            var giftCodes = await this.GeneralService.FindBy().OrderByDesc("id").Limit(100).GetAsync<GiftCodeEntity>();
            return Response(giftCodes);
        }
    }
}
