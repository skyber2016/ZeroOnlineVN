using API.Common;
using API.Configurations;
using API.Cores;
using API.Cores.Exceptions;
using API.DTO.User.Request;
using API.DTO.User.Responses;
using API.Entities;
using API.Helpers;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace API.Controllers
{
    public class UserController : GeneralController<AccountEntity>
    {
        [Dependency]
        public IGeneralService<AccountEntity> AccountService { get; set; }
        [Dependency]
        public IGeneralService<UserEntity> UserEntService { get; set; }
        [Dependency]
        public IAuthService AuthService { get; set; }
        [Dependency]
        public IGeneralService<StatisticEntity> StatisticService { get; set; }
        [Dependency]
        public IGeneralService<BotMessageEntity> BotMessageService { get; set; }
        [Dependency]
        public IGeneralService<WebLogEntity> WebLogService { get; set; }
        [Dependency]
        public IOptions<AppSettings> AppSettings { get; set; }

        [HttpGet]
        [Route("Money")]
        public async Task<IActionResult> Money()
        {
            var currentUser = this.UserService.GetCurrentUser();
            var user = await this.AccountService.SingleBy(new { id = currentUser.Id });
            return Response<UserGetMoneyResponse>(user);
        }

        [HttpPost]
        [Route("CoinToZP")]
        public async Task<IActionResult> CoinToZp(CoinToZpRequest request)
        {
            if(request.Money == null)
            {
                throw new BadRequestException("Bạn phải nhập số tiền muốn quy đổi sang ZPs");
            }
            var account = await this.AccountService.SingleBy(new { id = this.UserService.GetCurrentUser().Id });
            if(account.WebMoney < request.Money)
            {
                throw new BadRequestException("Số tiền quy đổi lớn hơn số Coin hiện có");
            }
            if(request.Money <= 0)
            {
                throw new BadRequestException("Số tiền quy đổi phải lớn hơn 0");
            }
            
            if(!account.MoneyValid())
            {
                throw new BadRequestException("Số tiền không hợp lệ");
            }
            var upgradeVIP = false;
            await this.UnitOfWork.CreateTransaction(async tran =>
            {
                var builder = new StringBuilder();
                builder.AppendLine("");
                builder.AppendLine("-------- **ĐỔI TIỀN** --------");
                builder.AppendLine($"**Tài khoản:** {this.UserService.GetCurrentUser().Username}");
                builder.AppendLine($"**Số tiền đổi:** {request.Money.Value.ToString("#,##0")}");
                builder.AppendLine($"**ZCoin hiện có:** {account.WebMoney.ToString("#,##0")}");
                account.WebMoney -= request.Money.Value;
                account.WebMoneyUsing += request.Money.Value;
                if (account.WebMoneyUsing >= 12000000 && account.VIP < 6)
                {
                    account.VIP = 6;
                    upgradeVIP = true;
                }
                else if(account.WebMoneyUsing >= 8000000 && account.VIP < 5)
                {
                    account.VIP = 5;
                    upgradeVIP = true;
                }
                else if(account.WebMoneyUsing >= 6000000 && account.VIP < 4)
                {
                    account.VIP = 4;
                    upgradeVIP = true;
                }
                else if(account.WebMoneyUsing >= 2000000 && account.VIP < 3)
                {
                    account.VIP = 3;
                    upgradeVIP = true;
                }
                else if(account.WebMoneyUsing >= 1000000 && account.VIP < 2)
                {
                    account.VIP = 2;
                    upgradeVIP = true;
                }
                else if(account.WebMoneyUsing >= 500000 && account.VIP < 1)
                {
                    account.VIP = 1;
                    upgradeVIP = true;
                }
                var user = await this.AuthService.GetCQUser(tran);
                if(user == null)
                {
                    throw new BadRequestException("Vui lòng tạo nhân vật trước");
                }
                var statics = await this.StatisticService.SingleBy(new
                {
                    iduser = user.Id.Value,
                    event_type = this.AppSettings.Value.EventType_Statistic
                }, tran);
                if (statics == null)
                {
                    statics = new StatisticEntity
                    {
                        Data = request.Money.Value,
                        EventType = this.AppSettings.Value.EventType_Statistic,
                        IdUser = user.Id.Value,
                        OwnerType = 0,
                        EventTime = 0
                    };
                    await this.StatisticService.AddAsync(statics, tran);
                }
                else
                {
                    if(statics.Data > 0)
                    {
                        throw new BadRequestException("Vui lòng nhận ZPs đã quy đổi trước đó");
                    }
                    statics.Data += request.Money.Value;
                    statics.EventTime = 0;
                    await this.StatisticService.UpdateAsync(statics, tran);
                }
                account.CheckSum = account.GetCheckSum();
                await this.AccountService.UpdateAsync(account, tran);
                builder.AppendLine($"**ZCoin còn lại:** {account.WebMoney.ToString("#,##0")}");
                builder.AppendLine($"**Thời gian:** {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                if (upgradeVIP)
                {
                    builder.AppendLine($"**Đạt mốc VIP mới:** {account.VIP}");
                }
                await this.WebLogService.AddAsync(new WebLogEntity
                {
                    AccountId = this.UserService.GetCurrentUser().Id,
                    Value = request.Money.Value,
                    Message = $"{this.UserService.GetCurrentUser().Username} vừa đổi {request.Money.Value.ToString("#,##0")} ZCoin sang ZPs thành công"
                }, tran);
                await this.BotMessageService.AddAsync(new BotMessageEntity
                {
                    Channel = ChannelConstant.DOI_TIEN.ToString(),
                    Message = builder.ToString().Base64Encode()
                }, tran);
            });
            if (upgradeVIP)
            {
                return Response(new { message = $"Chúc mừng bạn đã mốc VIP mới là {account.VIP}" });
            }
            return Response();
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            var currentUser = this.UserService.GetCurrentUser();
            var oldPassword = MD5Helper.HashPassword(request.OldPassword);
            var account = await this.AccountService.SingleBy(new
            {
                name = currentUser.Username,
                password = oldPassword
            });
            if(account == null)
            {
                throw new BadRequestException("Mật khẩu cũ không đúng");
            }
            if(request.NewPassword.Length < 6)
            {
                throw new BadRequestException("Mật khẩu mới phải có ít nhất 6 ký tự");
            }
            var newPassword = MD5Helper.HashPassword(request.NewPassword);
            if(newPassword == oldPassword)
            {
                throw new BadRequestException("Mật khẩu mới và mật khẩu cũ không được trùng nhau");
            }
            account.Password = newPassword;
            await this.AccountService.UpdateAsync(account);
            this.Logger.Info($"{currentUser.Username} đã đổi mật khẩu thành công");
            await this.AuthService.RevokeToken(currentUser.Id);
            return Response();
        }

    }
}
