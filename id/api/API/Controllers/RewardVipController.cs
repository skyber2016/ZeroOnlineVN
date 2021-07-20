using API.Common;
using API.Cores;
using API.Cores.Exceptions;
using API.DTO.RewardVip.Requests;
using API.DTO.RewardVip.Responses;
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
    public class RewardVipController : GeneralController<RewardVipEntity>
    {
        [Dependency]
        public IGeneralService<UserEntity> UserEntService { get; set; }
        [Dependency]
        public IGeneralService<CqBonusEntity> BonusService { get; set; }
        [Dependency]
        public IGeneralService<AccountEntity> AccountService { get; set; }
        [Dependency]
        public IGeneralService<BotMessageEntity> BotMessageService { get; set; }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var currentUser = this.UserService.GetCurrentUser();
            var user = await this.UserEntService.SingleBy(new
            {
                account_id = currentUser.Id
            });
            if(user == null)
            {
                throw new BadRequestException("Vui lòng tạo nhân vật trước khi thực hiện");
            }
            var account = await this.AccountService.SingleBy(new
            {
                id = currentUser.Id
            });
            if(account == null)
            {
                throw new UnAuthorizeException();
            }
            var rewards = await this.GeneralService.FindBy(new
            {
                user_id = user.Id
            }).GetAsync<RewardVipEntity>();
            var response = Enumerable.Range(1, 6).Select(x =>
            {
                var status = 0; // chua dat
                if(rewards.Any(a=>a.Vip == x))
                {
                    status = 1; // đã nhận
                }
                else if(account.VIP >= x)
                {
                    status = 2;
                }
                return new RewardVipGetResponse
                {
                    Id = x,
                    Name = $"Mốc VIP {x}",
                    Status = status
                };
            });
            return Response(response);
        }
        [HttpPost]
        public async Task<IActionResult> Create(RewardVipCreateRequest request)
        {
            var currentUser = this.UserService.GetCurrentUser();
            var user = await this.UserEntService.SingleBy(new
            {
                account_id = currentUser.Id
            });
            if(user == null)
            {
                throw new BadRequestException("Vui lòng tạo nhân vật trước khi thực hiện");
            }
            var account = await this.AccountService.SingleBy(new
            {
                id = currentUser.Id
            });
            if(account == null)
            {
                throw new UnAuthorizeException();
            }
            if(account.VIP > 6)
            {
                throw new BadRequestException("VIP không hợp lệ");
            }
            var rewards = await this.GeneralService.FindBy(new
            {
                user_id = user.Id,
                vip = request.Id
            }).GetAsync<RewardVipEntity>();
            if (rewards.Any())
            {
                throw new BadRequestException("Xin lỗi, mỗi phần thưởng chỉ có thể nhận 1 lần duy nhất!");
            }
            if(account.VIP < request.Id)
            {
                throw new BadRequestException("Xin lỗi, bạn chưa đủ điều kiện để nhận phần thưởng này!");
            }

            await this.BonusService.AddAsync(new CqBonusEntity
            {
                UserId = account.Id.Value,
                ActionId = CommonContants.ActionRewardVip[request.Id]
            });
            await this.GeneralService.AddAsync(new RewardVipEntity
            {
                UserId = user.Id.Value,
                Vip = request.Id,
                CreatedDate = DateTime.Now
            });
            var builder = new StringBuilder();
            builder.AppendLine(string.Empty);
            builder.AppendLine("--------- **NHẬN QUÀ VIP** ---------");
            builder.AppendLine($"**Tên tài khoản:** {account.Username}");
            builder.AppendLine($"**Nhận mốc VIP:** {request.Id}");
            builder.AppendLine($"**Thời gian:** {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
            await this.BotMessageService.AddAsync(new BotMessageEntity
            {
                Message = builder.ToString().Base64Encode()
            });
            return Response();
        }
    }
}
