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
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace API.Controllers
{
    public class WheelController : GeneralController<WheelLogEntity>
    {
        private static Random Rand = new Random();

        [Dependency]
        public IOptions<WheelSetting> WheelSetting { get; set; }
        [Dependency]
        public IGeneralService<AccountEntity> AccountService { get; set; }
        [Dependency]
        public IGeneralService<BotMessageEntity> BotService { get; set; }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            var currentUser = this.UserService.GetCurrentUser();
            var acc = await this.AccountService.SingleBy(new
            {
                id = currentUser.Id
            });
            if(acc == null)
            {
                throw new BadRequestException("Tài khoản không tồn tại");
            }
            if(acc.Wheel <= 0)
            {
                throw new BadRequestException("Bạn không có lượt quay nào");
            }
            var items = this.WheelSetting.Value.Items;
            var listRandom = new List<WheelItem>();
            foreach (var item in items)
            {
                for (int i = 0; i < item.Rate; i++)
                {
                    listRandom.Add(item);
                }
            }
            listRandom.Shuffle();
            var itemRand = listRandom.ItemRandom();
            switch (itemRand.Code)
            {
                case "one_more_time":
                    acc.Wheel += 1;
                    break;
                case "20k_wcoin":
                    acc.WebMoney += 20000;
                    acc.CheckSum = acc.GetCheckSum();
                    break;
                default:
                    break;
            }
            acc.Wheel -= 1;
            await this.AccountService.UpdateAsync(acc);
            var log = new WheelLogEntity
            {
                AccountId = currentUser.Id,
                CreatedDate = DateTime.Now,
                Desc = itemRand.Name
            };
            await this.GeneralService.AddAsync(log);
            var response = new WheelCreateResponse
            {
                Message = itemRand.Name,
                WheelRemain = acc.Wheel
            };
            this.Logger.Info($"Tài khoản {acc.Username} vừa quay {itemRand.Name}");
            var builder = new StringBuilder();
            builder.AppendLine("-------- **VÒNG QUAY** --------");
            builder.AppendLine($"**Tài khoản:** {currentUser.Username}");
            builder.AppendLine($"**Code:** {itemRand.Code}");
            builder.AppendLine($"**Tên:** {itemRand.Name}");
            builder.AppendLine($"**Thời gian:** {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
            await this.BotService.AddAsync(new BotMessageEntity
            {
                Channel = ChannelConstant.WHEEL.ToString(),
                Message = builder.ToString().Base64Encode()
            });
            return Response(response);
        }
    }
}
