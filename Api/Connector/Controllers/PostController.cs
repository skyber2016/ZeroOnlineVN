using Forum_API.Cores;
using Forum_API.Cores.Exceptions;
using Forum_API.DTO.Base;
using Forum_API.DTO.Post.Requests;
using Forum_API.DTO.Post.Responses;
using Forum_API.Entities;
using Forum_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Unity;

namespace Forum_API.Controllers
{
    public class PostController : GeneralController<PostEntity, PostGetResponse, PostCreateRequest, PostCreateResponse, PostUpdateRequest, PostUpdateResponse>
    {
        [Dependency]
        public IGeneralService<TopicEntity> TopicService { get; set; }
        public override async Task<IActionResult> Create(PostCreateRequest request)
        {
            var hasTopic = await this.TopicService.FindBy(x => x.Id == request.TopicId).AnyAsync();
            if (!hasTopic)
            {
                throw new NotFoundException();
            }
            return await base.Create(request);
        }

        [HttpGet]
        [Route("Topic/{topicId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByTopic(long topicId, int pageNumber = 1, int pageSize = 20)
        {
            var data = this.GeneralService.FindBy(x => x.TopicId == topicId);
            var count = await data.CountAsync();
            var resp = await data.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return Response(new PaginationResponse<PostGetByTopicResponse>
            {
                Data = resp.Select(x => Mapper.Map<PostGetByTopicResponse>(x)),
                TotalRecords = count
            });
        }
    }
}
