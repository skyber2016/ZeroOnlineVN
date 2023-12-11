using API.Common;
using API.Configurations;
using API.Cores;
using API.Cores.Exceptions;
using API.DTO.Wheel.Responses;
using API.Entities;
using API.Helpers;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace API.Controllers
{
    public class WheelController : GeneralController<WheelLogEntity>
    {

        [Dependency]
        public IOptions<WheelSetting> WheelSetting { get; set; }
        [Dependency]
        public IGeneralService<AccountEntity> AccountService { get; set; }
        [Dependency]
        public IGeneralService<BotMessageEntity> BotService { get; set; }
        [Dependency]
        public IGeneralService<CqBonusEntity> BonusService { get; set; }
        [Dependency]
        public IGeneralService<WheelLogEntity> WheelLogService { get; set; }
        [Dependency]
        public IGeneralService<UserEntity> CqUserService { get; set; }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            var currentUser = this.UserService.GetCurrentUser();
            var acc = await this.AccountService.SingleBy(new
            {
                id = currentUser.Id
            });
            if (acc == null)
            {
                throw new BadRequestException("Tài khoản không tồn tại");
            }
            if (acc.Wheel <= 0)
            {
                throw new BadRequestException("Bạn không có lượt quay nào");
            }
            var cqUser = await this.CqUserService.SingleBy(new
            {
                account_id = currentUser.Id
            });
            if (cqUser == null)
            {
                throw new BadRequestException("Vui lòng tạo nhân vật trước");
            }
            var goodLuck = "good_luck";
            var items = this.WheelSetting.Value.Items;
            var listRandom = items.GetItems();
            var itemRand = listRandom.ItemRandom();
            switch (itemRand.Code)
            {
                case "one_more_time":
                    acc.Wheel += 1;
                    break;
                case "20k_zcoin":
                    acc.WebMoney += 20000;
                    acc.CheckSum = acc.GetCheckSum();
                    break;
                default:
                    break;
            }
            acc.Wheel -= 1;

            var index = this.WheelSetting.Value.Items.IndexOf(itemRand);
            var deg = (360 / 8) * (-index);
            var minDeg = deg - 15;
            var maxDeg = deg + 15;

            var sql = new StringBuilder();
            var log = new WheelLogEntity
            {
                AccountId = currentUser.Id,
                CharName = cqUser.Name,
                CreatedDate = DateTime.Now,
                Desc = itemRand.Name.Base64Encode()
            };
            if(itemRand.Code != goodLuck)
            {
                await this.GeneralService.AddAsync(log);
            }
            if (itemRand.ActionId != 0)
            {
                await this.BonusService.AddAsync(new CqBonusEntity
                {
                    ActionId = itemRand.ActionId,
                    UserId = currentUser.Id
                });
            }
            await this.AccountService.UpdateAsync(acc);

            this.Logger.Info($"Tài khoản {acc.Username} vừa quay {itemRand.Name}");
            if(itemRand.Code != goodLuck)
            {
                var builder = new StringBuilder();
                builder.AppendLine("-------- **VÒNG QUAY** --------");
                builder.AppendLine($"**Tài khoản:** {currentUser.Username}");
                builder.AppendLine($"**Code:** {itemRand.Code}");
                builder.AppendLine($"**Tên:** {itemRand.Name}");
                builder.AppendLine($"**Thời gian:** {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                //await this.BotService.AddAsync(new BotMessageEntity
                //{
                //    Channel = ChannelConstant.WHEEL.ToString(),
                //    Message = builder.ToString().Base64Encode()
                //});
            }
            
            var histories = await this.WheelLogService.FindBy().OrderByDesc("id").Limit(22).GetAsync<WheelLogEntity>();
            var response = new WheelCreateResponse
            {
                Message = itemRand.Name,
                WheelRemain = acc.Wheel,
                Deg = RandomNumberGenerator.GetInt32(minDeg, maxDeg),
                Histories = histories.Select(x =>
                {
                    var map = Mapper.Map<WheelLogResponse>(x);
                    map.Desc = map.Desc.Base64Decode();
                    return map;
                }),
            };
            return Response(response);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var currentUser = this.UserService.GetCurrentUser();
            var acc = await this.AccountService.SingleBy(new
            {
                id = currentUser.Id
            });
            if (acc == null)
            {
                throw new BadRequestException("Tài khoản này không tồn tại");
            }
            var logs = await this.WheelLogService.FindBy().OrderByDesc("id").Limit(22).GetAsync<WheelLogEntity>();
            var response = new WheelGetResponse
            {
                Histories = logs.Select(x =>
                {
                    var map = Mapper.Map<WheelLogResponse>(x);
                    map.Desc = map.Desc.Base64Decode();
                    return map;
                }),
                WheelCount = acc.Wheel
            };
            return Response(response);
        }
    }
}
