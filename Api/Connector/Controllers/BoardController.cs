using Forum_API.Cores;
using Forum_API.Cores.WebSockets.Handler;
using Forum_API.DTO.Base;
using Forum_API.DTO.Board.Responses;
using Forum_API.DTO.Role.Responses;
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
    public class BoardController : BaseController
    {
        [Dependency]
        public IGeneralService<PostEntity> PostService { get; set; }
        [Dependency]
        public IGeneralService<UserEntity> UserEntService { get; set; }
        [Dependency]
        public IGeneralService<RoleEntity> RoleService { get; set; }
        [Dependency]
        public MemberOnlineHandler MemberSocket { get; set; }
        [Dependency]
        public IGeneralService<RefreshTokenEntity> RefreshTokenService { get; set; }

        [HttpGet]
        [Route("Stats")]
        [AllowAnonymous]
        public async Task<IActionResult> Stats()
        {
            var totalPost = await this.PostService.FindBy(_ => true).CountAsync();
            var totalUser = await this.UserEntService.FindBy(_ => true).CountAsync();
            var newesMember = await this.UserEntService.FindBy(_ => true).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            return Response(new
            {
                totalPost = totalPost.ToString("#,##0"),
                totalUser = totalUser.ToString("#,##0"),
                newstMember = newesMember.Username
            });
        }

        [HttpGet]
        [Route("Level")]
        [AllowAnonymous]
        public async Task<IActionResult> Level()
        {
            var roles = await this.RoleService.FindBy(x=>x.Prioritize > 0).OrderBy(x=>x.Prioritize).ToListAsync();
            return Response<RoleStatResponse>(roles);
        }

        [HttpGet]
        [Route("Online")]
        [AllowAnonymous]
        public async Task<IActionResult> Online()
        {
            var membersKey = MemberSocket.WebSocketConnectionManager.GetAll().Select(x => x.Key).Select(x=> x.Replace("MEMBER_ONLINE_",string.Empty)).ToList();
            var users = await this.RefreshTokenService.FindBy(x => membersKey.Contains(x.Token)).ToListAsync();
            var resp = new BoardOnlineResponse
            {
                Guest = membersKey.Count - users.Count,
                Users = users.Select(x => Mapper.Map<BaseUserGetResponse>(x.User))
            };
            return Response(resp);
        }
    }
}
