using API.Common;
using API.Cores;
using API.Cores.Exceptions;
using API.DTO.RewardShop.Requests;
using API.DTO.RewardShopItem.Responses;
using API.Entities;
using API.Helpers;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Unity;

namespace API.Controllers
{
    public class RewardShopController : GeneralController<RewardShopEntity>
    {
        [Dependency]
        public IGeneralService<RewardShopItemEntity> ItemService { get; set; }
        [Dependency]
        public IGeneralService<CqBonusEntity> BonusService { get; set; }
        [Dependency]
        public IGeneralService<WebShopEntity> WebShopService { get; set; }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var items = await this.ItemService.GetAll();
            items = items.OrderBy(x => x.CreatedDate);
            var time = DateTime.Now;
            DayOfWeek today = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            var monday = time.AddDays(-((int)today - 1)).Date;
            var sunday = monday.AddDays(7).Date.AddSeconds(-1);
            if(today == DayOfWeek.Sunday)
            {
                sunday = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                monday = sunday.Date.AddDays(-6);
            }
            var currentUser = this.UserService.GetCurrentUser();
            var money = await WebShopService.FindBy(new
            {
                user_id = currentUser.Id
            }).WhereBetween("created_date", monday, sunday).GetAsync<WebShopEntity>();
            long totalMoney = money.Sum(x => x.Price);
            var his = await this.GeneralService.FindBy().WhereBetween("created_date", monday, sunday).GetAsync<RewardShopEntity>();
            var res = RewardShopConstant.Reward.Select(x =>
            {
                var status = 0;
                var item = items.Where(w => w.Group == x.Key).Select(s => {
                    var m = Mapper.Map<RewardShopItemGetResponse>(s);
                    m.Name = m.Name.Base64Decode();
                    return m;
                });
                float percent = 0;
                percent = (float)totalMoney / RewardShopConstant.Reward[x.Key] * 100;
                if(percent > 100)
                {
                    percent = 100;
                }   

                if (totalMoney >= RewardShopConstant.Reward[x.Key])
                {
                    if (his.Any(a => a.ItemId == x.Key && a.UserId == currentUser.Id))
                    {
                        status = 1;
                    }
                    else
                    {
                        status = 2;
                    }
                }
                else
                {
                    status = 0;
                }
                if (!item.Any())
                {
                    status = 0;
                }
                return new
                {
                    group = x.Key,
                    items = item,
                    status = status,
                    percent = percent,
                };
            });
            return Response(new
            {
                current = totalMoney.ToString("#,##0"),
                rewards = res
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(RewardShopCreateRequest request)
        {
            var time = DateTime.Now;
            DayOfWeek today = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            var monday = time.AddDays(-((int)today - 1)).Date;
            var sunday = monday.AddDays(7).Date.AddSeconds(-1);
            if (today == DayOfWeek.Sunday)
            {
                sunday = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
                monday = sunday.Date.AddDays(-6);
            }
            var currentUser = this.UserService.GetCurrentUser();
            var money = await WebShopService.FindBy(new
            {
                user_id = currentUser.Id
            }).WhereBetween("created_date", monday, sunday).GetAsync<WebShopEntity>();
            long totalMoney = money.Sum(x => x.Price);
            if(totalMoney < RewardShopConstant.Reward[request.Group])
            {
                throw new BadRequestException("Bạn chưa tích lũy đủ điểm");
            }
            var his = await this.GeneralService.FindBy(new {
                item_id = request.Group,
                user_id = currentUser.Id
            }).WhereBetween("created_date", monday, sunday).GetAsync<RewardShopEntity>();

            if(his.Any())
            {
                throw new BadRequestException("Bạn đã nhận vật phẩm này rồi");
            }
            var items = await this.ItemService.FindBy(new {
                group = request.Group
            }).GetAsync<RewardShopItemEntity>();
            if(!items.Any())
            {
                throw new BadRequestException("Vật phẩm này không tồn tại");
            }
            await this.GeneralService.AddAsync(new RewardShopEntity
            {
                ItemId = request.Group,
                UserId = currentUser.Id
            });
            foreach (var item in items)
            {
                await BonusService.AddAsync(new CqBonusEntity
                {
                    ActionId = item.ActionId,
                    UserId = currentUser.Id
                });
            }
            return Response();
        }
    }
}
