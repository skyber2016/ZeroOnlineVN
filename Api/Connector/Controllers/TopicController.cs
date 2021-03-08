using Forum_API.Cores;
using Forum_API.Cores.Exceptions;
using Forum_API.Cores.WebSockets.Handler;
using Forum_API.DTO.Base;
using Forum_API.DTO.Topic.Requests;
using Forum_API.DTO.Topic.Responses;
using Forum_API.Entities;
using Forum_API.Helpers;
using Forum_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using Unity;

namespace Forum_API.Controllers
{
    public class TopicController : GeneralController<TopicEntity, TopicGetResponse, TopicCreateRequest, TopicCreateResponse, TopicUpdateRequest, TopicUpdateResponse>
    {
        [Dependency]
        public IGeneralService<CategoryEntity> CategoryService { get; set; }
        [Dependency]
        public IGeneralService<TopicEntity> TopicService { get; set; }
        [Dependency]
        public TopicHandler TopicHandler { get; set; }

        public override async Task<IActionResult> Create(TopicCreateRequest request)
        {
            var hasCategory = await this.CategoryService.FindBy(x => x.Id == request.CategoryId).AnyAsync();
            if (!hasCategory)
            {
                throw new NotFoundException();
            }
            var entity = Mapper.Map<TopicEntity>(request);
            entity = await this.GeneralService.AddAsync(entity);
            var dataInserted = this.GeneralService.SingleBy(x => x.Id == entity.Id);
            var response = Mapper.Map<TopicCreateResponse>(dataInserted);
            await this.TopicHandler.SendMessageToAllAsync(JsonConvert.SerializeObject(response));
            return Response(response);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Category/{categoryId}")]
        public async Task<IActionResult> GetByCategoryId(long categoryId, int pageNumber = 1, int pageSize = 20)
        {
            var topics = this.GeneralService.FindBy(x => x.CategoryId == categoryId);
            var count = await topics.CountAsync();
            var data = await topics.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            var resp = data.Select(x => {
                var mapper = Mapper.Map<TopicGetResponse>(x);
                mapper.Replies = x.Posts.Count;
                return mapper;
            });
            return Response(new PaginationResponse<TopicGetResponse>
            {
                TotalRecords = count,
                Data = resp
            });
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("LastTopics")]
        public async Task<IActionResult> LastTopics()
        {
            var topics = await this.TopicService.FindBy(_ => true).OrderByDescending(x => x.Id).Take(15).ToListAsync();
            var response = topics.Select(x => Mapper.Map<TopicGetResponse>(x)).Select(x =>
            {
                x.Slug = x.Name.ToUrl();
                if (x.Name.Length > 32)
                {
                    x.Name = string.Join("", x.Name.Take(32).ToArray()) + " ...";
                }
                return x;
            });
            return Response(response);
        }
    }
}
