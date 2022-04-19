using API.Cores;
using API.DTO.Ranking.Responses;
using API.Entities;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlKata.Execution;
using System.Linq;
using System.Threading.Tasks;
using Unity;

namespace API.Controllers
{
    public class RankingController : GeneralController<UserEntity>
    {
        [Dependency]
        public IGeneralService<UserEntity> UserEntService { get; set; }
        [Dependency]
        public IGeneralService<SyndicateEntity> SyndicateService { get; set; }

        [HttpGet]
        [Route("Power")]
        public async Task<IActionResult> Power()
        {
            var users = await this.UserEntService.FindBy().Join("account", "cq_user.account_id","account.id").Select("cq_user.*", "account.VIP").OrderByDesc("Battle_lev").Limit(110).GetAsync<UserEntity>();
            return Response<RankingPowerGetResponse>(users.Where(x=>!x.Name.EndsWith("[PM]")).Take(100));
        }

        [HttpGet]
        [Route("Syndicate")]
        public async Task<IActionResult> Syndicate()
        {
            var syncs = await this.SyndicateService.FindBy().OrderByDesc("rank","money").GetAsync<SyndicateEntity>();
            return Response(syncs);
        }
    }
}
