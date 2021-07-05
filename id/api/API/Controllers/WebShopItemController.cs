using API.Attributes;
using API.Cores;
using API.Cores.Exceptions;
using API.DTO.WebShopItem.Requests;
using API.DTO.WebShopItem.Responses;
using API.Entities;
using API.Helpers;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlKata.Execution;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Unity;

namespace API.Controllers
{
    public class WebShopItemController : GeneralController<WebShopItemEntity>
    {
        [Dependency]
        public IGeneralService<AccountEntity> AccountService { get; set; }
        [Dependency]
        public IGeneralService<CqBonusEntity> BonusService { get; set; }
        [Dependency]
        public IGeneralService<UserEntity> UserEntService { get; set; }
        [Dependency]
        public IGeneralService<BotMessageEntity> BotService { get; set; }
        [Dependency]
        public IWebHostEnvironment Env { get; set; }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var items = await this.GeneralService.FindBy().OrderByDesc("id").GetAsync<WebShopItemEntity>();
            var response = items.Select(x => {
                var map = Mapper.Map<WebShopItemGetResponse>(x);
                map.Name = map.Name.Base64Decode();
                return map;
            });
            return Response(response);
        }

        [HttpPost]
        [Admin]
        public async Task<IActionResult> Create([FromForm]WebShopItemCreateRequest request)
        {
            var exist = await this.GeneralService.SingleBy(new
            {
                id = request.Id
            });
            if(exist != null)
            {
                throw new BadRequestException("Vật phẩm này đã tồn tại");
            }
            
            
            var entity = Mapper.Map<WebShopItemEntity>(request);
            entity.Name = entity.Name.Base64Encode();
            await this.GeneralService.AddAsync(entity);
            return Response();
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("upload")]
        [Admin]
        public async Task<IActionResult> UploadFile([FromForm]IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName);
            var name = Guid.NewGuid().ToString();
            var folderName = Path.Combine("wwwRoot", "images-upload");
            var pathToSave = Path.Combine(Env.ContentRootPath, folderName);
            var fileName = name + ext;
            var fullPath = Path.Combine(pathToSave, fileName);
            var dbPath = $"/images-upload/{fileName}";
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return Response(dbPath);
        }

        [HttpPut]
        [Admin]

        public async Task<IActionResult> Update([FromForm]WebShopItemUpdateRequest request)
        {
            var entity =  await this.GeneralService.SingleBy(new
            {
                id = request.Id
            });
            if(entity == null)
            {
                throw new BadRequestException("Vật phẩm này không tồn tại");
            }
            entity = Mapper.Map(request, entity);
            entity.Name = entity.Name.Base64Encode();
            await this.GeneralService.UpdateAsync(entity);
            return Response();
        }

        [HttpDelete]
        [Admin]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await this.GeneralService.SingleBy(new
            {
                id = id
            });
            if (entity == null)
            {
                throw new BadRequestException("Vật phẩm này không tồn tại");
            }
            await this.GeneralService.Delete(entity);
            return Response();
        }
    }
}
