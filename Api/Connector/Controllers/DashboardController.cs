using Forum_API.Cores;
using Forum_API.DTO.Area.Responses;
using Forum_API.DTO.Dashboard.Responses;
using Forum_API.DTO.ShoutBox.Responses;
using Forum_API.DTO.Topic.Responses;
using Forum_API.DTO.User.Responses;
using Forum_API.Entities;
using Forum_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity;

namespace Forum_API.Controllers
{
    public class DashboardController : GeneralController<AreaEntity>
    {
        [Dependency]
        public IGeneralService<ShoutBoxEntity> ShoutBoxService { get; set; }
        [Dependency]
        public IGeneralService<TopicEntity> TopicService { get; set; }
        [Dependency]
        public IGeneralService<UserEntity> UserEntService { get; set; }
        [Dependency]
        public IGeneralService<PostEntity> PostService { get; set; }
        [Dependency]
        public IGeneralService<RefreshTokenEntity> RefreshTokenService { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var shoutBox = await this.ShoutBoxService
                .FindBy(x => true)
                .OrderByDescending(x=>x.Id)
                .Take(30)
                .ToListAsync()
                ;
            var areas = await this.GeneralService
                .FindBy(x => true)
                .Include(x=>x.Categories)
                .ThenInclude(x=>x.Topics)
                .OrderBy(x => x.Id)
                .ToListAsync();
            var lastTopics = await this.TopicService.FindBy(x => true)
                .Include(x => x.Posts)
                .OrderByDescending(x => x.CreatedDate)
                .Take(30)
                .ToListAsync()
                ;
            var newestMember = await this.UserEntService.FindBy(x => true).OrderBy(x => x.Id).LastOrDefaultAsync();
            var response = new DashboardGetResponse
            {
                ShoutBoxs = shoutBox.Select(x => Mapper.Map<ShoutBoxGetResponse>(x)),
                Areas = areas.Select(x => Mapper.Map<AreaGetResponse>(x)),
                LastTopics = lastTopics.Select(x=> Mapper.Map<TopicGetResponse>(x)),
                TotalMembers = await this.UserEntService.FindBy(x=>true).LongCountAsync(),
                TotalPosts = await this.PostService.FindBy(x => true).LongCountAsync(),
                NewestMember = Mapper.Map<UserGetResponse>(newestMember),
                TotalTopic = await this.TopicService.FindBy(x=>true).LongCountAsync()
            };
            return Response(response);

        }
    }
}
