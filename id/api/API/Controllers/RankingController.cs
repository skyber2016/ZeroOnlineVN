using API.Cores;
using API.DTO.Ranking.Responses;
using API.Entities;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity;

namespace API.Controllers
{
    public class RankingController : GeneralController<UserEntity>
    {
        [Dependency]
        public IGeneralService<UserEntity> UserEntService { get; set; }

        [HttpGet]
        [Route("Power")]
        public async Task<IActionResult> Power()
        {
            var users = await this.UserEntService.FindBy().LeftJoin("account", "cq_user.account_id","account.id").Select("cq_user.*", "account.VIP").OrderByDesc("Battle_lev").Limit(110).GetAsync<UserEntity>();
            return Response<RankingPowerGetResponse>(users.Where(x=>!x.Name.EndsWith("[PM]")).Take(100));
        }
    }
}
