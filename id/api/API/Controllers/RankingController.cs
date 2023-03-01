using API.Configurations;
using API.Cores;
using API.DTO.Ranking.Responses;
using API.Entities;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SqlKata.Execution;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity;

namespace API.Controllers
{
    public class RankingController : GeneralController<UserEntity>
    {
        [Dependency]
        public IGeneralService<StatisticEntity> StatisticService { get; set; }
        [Dependency]
        public IGeneralService<UserEntity> UserEntService { get; set; }
        [Dependency]
        public IGeneralService<SyndicateEntity> SyndicateService { get; set; }
        [Dependency]
        public IOptions<List<StatisticSetting>> StatisticSettings { get; set; }

        [HttpGet]
        [Route("Power")]
        public async Task<IActionResult> Power()
        {
            var users = await this.UserEntService.FindBy()
                .Join("account", "cq_user.account_id","account.id")
                .Select("cq_user.*", "account.VIP")
                .OrderByDesc("Battle_lev")
                .Limit(110)
                .GetAsync<UserEntity>();
            var idUsers = users.Where(x => !x.Name.EndsWith("[PM]")).Select(x => x.Id).ToArray();
            var statistics = await this.StatisticService.FindBy()
                .WhereIn("event_type", StatisticSettings.Value.Select(x=>x.EventType))
                .GetAsync<StatisticEntity>()
                ;
            var res = users.Where(x => !x.Name.EndsWith("[PM]")).Take(100).Select(x =>
            {
                var mapping = Mapper.Map<RankingPowerGetResponse>(x);
                mapping.Statistics = StatisticSettings.Value.Select(s =>
                {
                    s.Data = statistics.FirstOrDefault(w => w.IdUser == x.Id && w.EventType == s.EventType)?.Data ?? 0;
                    return s;
                }).ToList();
                return mapping;
            });
            return Response(new
            {
                Columns = StatisticSettings.Value,
                Rankings = res
            });
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
