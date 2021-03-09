using API.Entities;
using Microsoft.AspNetCore.Mvc;
using SqlKata.Execution;
using System.Threading.Tasks;
using Unity;
using API.Common;
using API.Cores.Exceptions;
using API.Helpers;
using API.Services.Interfaces;

namespace API.Cores
{
    public class GeneralController<EntityModel> : BaseController where EntityModel : BaseEntity, new()
    {
        [Dependency]
        protected IGeneralService<EntityModel> GeneralService { get; set; }
        [Dependency]
        public IUserService UserService { get; set; }
        [Dependency]
        public ILoggerManager Logger { get; set; }
    }

    public class GeneralController<EntityModel, GetResponse> : GeneralController<EntityModel>
        where EntityModel : BaseEntity, new()
        where GetResponse: class,new()
    {
        /// <summary>
        /// API Lấy tất cả thông tin và có phân trang
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [FunctionMethod(FuncConstants.Get)]
        public virtual async Task<IActionResult> Get(int? Pagenumber,int? PageSize)
        {
            var data = this.GeneralService.FindBy();
            var result = await data.GetAsync<EntityModel>();
            return Response<GetResponse>(result);
        }

        /// <summary>
        /// Lấy thông tin của 1 entity
        /// </summary>
        /// <param name="id">ID của entity</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [FunctionMethod(FuncConstants.GetById)]
        public virtual async Task<IActionResult> GetById(int id)
        {
            var entity = this.GeneralService.SingleBy(new { id });
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
            var entity = await this.GeneralService.SingleBy(new { id= request.Id });
            if(entity == null)
            {
                throw new NotFoundException();
            }
            entity = Mapper.Map(request, entity);
            entity = await this.GeneralService.UpdateAsync(entity);
            return Response<UpdateResponse>(entity);
        }
    }

}
