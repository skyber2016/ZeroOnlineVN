using Forum_API.Common;
using Forum_API.Cores.Exceptions;
using Forum_API.DTO.Base;
using Forum_API.Entities;
using Forum_API.Helpers;
using Forum_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Unity;

namespace Forum_API.Cores
{
    public class GeneralController<EntityModel> : BaseController where EntityModel : BaseEntity, new()
    {
        [Dependency]
        public IGeneralService<EntityModel> GeneralService { get; set; }
        
    }

    public class GeneralController<EntityModel, GetResponse> : GeneralController<EntityModel>
        where EntityModel : BaseEntity, new()
        where GetResponse: class,new()
    {
        /// <summary>
        /// API Lấy tất cả thông tin và có phân trang
        /// </summary>
        /// <param name="pageNumber">Index của page hiện tại</param>
        /// <param name="pageSize">Số item trong 1 page, max = 100</param>
        /// <returns></returns>
        [HttpGet]
        [FunctionMethod(FuncConstants.Get)]
        public virtual async Task<IActionResult> Get(int pageNumber = 1, int pageSize = 10)
        {
            var maxPageSize = 500;
            if(pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }
            var skip = (pageNumber - 1) * pageSize;
            var data = this.GeneralService.FindBy(x=>!x.IsDeleted);
            var totalRecords = data.Count();
            var result = await data.OrderByDescending(x => x.CreatedDate).Skip(skip).Take(pageSize).ToListAsync();
            var baseResponse = new PaginationResponse<GetResponse>
            {
                TotalRecords = totalRecords,
                Data = result.Select(x => Mapper.Map<GetResponse>(x))
            };
            return Response(baseResponse);
        }

        /// <summary>
        /// Lấy thông tin của 1 entity
        /// </summary>
        /// <param name="id">ID của entity</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [FunctionMethod(FuncConstants.GetById)]
        public virtual async Task<IActionResult> GetById(long id)
        {
            var entity = this.GeneralService.SingleBy(x => x.Id == id && !x.IsDeleted);
            if(entity == null)
            {
                throw new NotFoundException();
            }
            return Response<GetResponse>(entity);
        }
    }

    public class GeneralController<EntityModel, GetResponse, CreateRequest, CreateResponse> : GeneralController<EntityModel, GetResponse>
        where EntityModel : BaseEntity, new()
        where GetResponse : class, new()
        where CreateResponse: class,new()
    {
        /// <summary>
        /// API Tạo entity
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [FunctionMethod(FuncConstants.Create)]
        public virtual async Task<IActionResult> Create(CreateRequest request)
        {
            var entity = Mapper.Map<EntityModel>(request);
            entity = await this.GeneralService.AddAsync(entity);
            return Response<CreateResponse>(entity);
        }
    }

    public class GeneralController<EntityModel, GetResponse, CreateRequest, CreateResponse, UpdateRequest, UpdateResponse> : GeneralController<EntityModel, GetResponse, CreateRequest, CreateResponse>
        where EntityModel : BaseEntity, new()
        where GetResponse : class, new()
        where CreateResponse : class, new()
        where UpdateResponse : class, new()
        where UpdateRequest : DTO.Base.UpdateRequest, new()
    {
        /// <summary>
        /// API Chỉnh sửa thông tin
        /// </summary>
        /// <param name="request">Các thông tin này sẽ được cập nhật lại</param>
        /// <returns>Trả về entity đã update</returns>
        [HttpPut]
        [FunctionMethod]
        public virtual async Task<IActionResult> Update(UpdateRequest request)
        {
            var entity = this.GeneralService.SingleBy(x => x.Id == request.Id && !x.IsDeleted);
            if(entity == null)
            {
                throw new NotFoundException();
            }
            entity = Mapper.Map(request, entity);
            entity = await this.GeneralService.UpdateAsync(entity);
            return Response<UpdateResponse>(entity);
        }

        [HttpDelete]
        [FunctionMethod]
        public virtual async Task<IActionResult> Delete([FromQuery]DTO.Base.UpdateRequest request)
        {
            var entity = this.GeneralService.SingleBy(x => x.Id == request.Id);
            if(entity == null)
            {
                throw new NotFoundException();
            }
            await this.GeneralService.Delete(entity);
            return Response<UpdateResponse>(entity);
        }
    }

}
