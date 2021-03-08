using Forum_API.Cores;
using Forum_API.DTO.Area.Requests;
using Forum_API.DTO.Area.Responses;
using Forum_API.DTO.Base;
using Forum_API.DTO.Topic.Responses;
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
    public class AreaController : GeneralController<AreaEntity, AreaGetResponse, AreaCreateRequest, AreaCreateResponse, AreaUpdateRequest, AreaUpdateResponse>
    { 
        [Dependency]
        public IGeneralService<PostEntity> PostService { get; set; }
        [AllowAnonymous]
        public override async Task<IActionResult> Get(int pageNumber = 1, int pageSize = 10)
        {
            var maxPageSize = 500;
            if (pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }
            var skip = (pageNumber - 1) * pageSize;
            var data = this.GeneralService.FindBy(x => !x.IsDeleted);
            var totalRecords = await data.CountAsync();
            var result = await data.OrderBy(x => x.Order).Skip(skip).Take(pageSize).ToListAsync();
            var response = result.Select(x => Mapper.Map<AreaGetResponse>(x)).ToList();
            var topicsId = result.SelectMany(x => x.Categories).Select(x => x.Topics.OrderByDescending(a => a.Id).Take(1)).SelectMany(x => x).ToList();
            var finnalData = response.Select(x =>
            {
                x.Categories = x.Categories.Select(cate =>
                {
                    var topicEntity = topicsId.FirstOrDefault(x => x.CategoryId == cate.Id);
                    if (topicEntity != null)
                    {
                        cate.LastTopic = Mapper.Map<TopicGetResponse>(topicEntity);
                        cate.LastTopic.Name = string.Join("", cate.LastTopic.Name.Take(20).ToArray()) + " ...";
                    }
                    return cate;
                });
                return x;
            });
            var baseResponse = new PaginationResponse<AreaGetResponse>
            {
                TotalRecords = totalRecords,
                Data = finnalData
            };
            return Response(baseResponse);
        }
    }
}
