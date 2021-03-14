using API.Common;
using API.Cores;
using API.Cores.Exceptions;
using API.DTO.Card.Requests;
using API.DTO.Card.Respones;
using API.Entities;
using API.Helpers;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SqlKata.Execution;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace API.Controllers
{
    public class CardController : GeneralController<CardEntity>
    {
        [Dependency]
        public ICardService CardService { get; set; }
        [Dependency]
        public IGeneralService<BotMessageEntity> BotMessageService { get; set; }
        [Dependency]
        public IGeneralService<AccountEntity> AccountService { get; set; }
        [Dependency]
        public IWebHostEnvironment Env { get; set; }
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(CardCreateResponse))]
        public async Task<IActionResult> Create(CardCreateRequest request)
        {
            var currentUser = this.UserService.GetCurrentUser();
            var entity = Mapper.Map<CardEntity>(request);
            entity.AccountId = currentUser.Id;
            if (request.Status == 0) //nap nhanh
            {
                entity = await this.CardService.CardCharge(entity);
                if (entity.Status == 100)
                {
                    var account = await this.AccountService.SingleBy(new { id = this.UserService.GetCurrentUser().Id });
                    account.WebMoney += request.Value;
                    await this.AccountService.UpdateAsync(account);
                }
            }
            await this.UnitOfWork.CreateTransaction(async trans =>
            {
                var builder = new StringBuilder();
                builder.AppendLine(string.Empty);
                if(request.Status == 0)
                {
                    builder.AppendLine($"--------------------**NẠP THẺ**--------------------");
                }
                else
                {
                    builder.AppendLine($"--------------------**NẠP THẺ CHẬM**--------------------");
                }
                builder.AppendLine($"**Mã giao dịch:** {entity.TranId}");
                builder.AppendLine($"**Tên tài khoản:** {currentUser.Username}");
                builder.AppendLine($"**Loại thẻ:** {CommonContants.CardType[entity.Type]}");
                builder.AppendLine($"**Seri:** {entity.Seri}");
                builder.AppendLine($"**Mã nạp:** {entity.Code}");
                builder.AppendLine($"**Mệnh giá:** {entity.Value.ToString("#,##0")}");
                if(request.Status == 0)
                {
                    builder.AppendLine($"**Mệnh giá thực tế:** {entity.Amount}");
                }
                builder.AppendLine($"**Trạng thái:** {entity.Message}");
                builder.AppendLine($"**Thời gian:** {entity.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss")}");
                if (request.Status != 0)
                {
                    builder.AppendLine($"**Lệnh xác nhận thành công:** /xacnhan {entity.TranId}");
                    builder.AppendLine($"**Lệnh xác nhận thẻ sai:** /thesai {entity.TranId} thẻ không chính xác");
                    entity.Amount = entity.Value.ToString();
                }
                entity.Message = entity.Message.Base64Encode();
                entity = await this.CardService.AddAsync(entity, trans);
                await this.BotMessageService.AddAsync(new BotMessageEntity
                {
                    Message = builder.ToString().Base64Encode()
                }, trans);
            });
            if(entity.Status != 100 && request.Status == 0)
            {
                throw new BadRequestException(entity.Message.Base64Decode());
            }
            return Response(new CardCreateResponse { Message = entity.Message.Base64Decode() });
        }

       [HttpGet]
       [Route("CardInfo")]
       public async Task<IActionResult> CardInfo()
        {
            using(var http = new HttpClient())
            {
                var result = await http.GetAsync("http://api.napthengay.com/Status.php");
                var content = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<List<CardStatusReponse>>(content);
                return Response(response.Where(x => x.Status == "1"));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var currentUser = this.UserService.GetCurrentUser();
            var cards = await this.CardService.FindBy(new { account_id = currentUser.Id }).OrderByDesc("id").GetAsync<CardEntity>();
            cards = cards.Select(x =>
            {
                x.Message = x.Message.Base64Decode();
                return x;
            });
            return Response<CardGetResponse>(cards);
        }

        [HttpPost]
        [Route("ConfirmBanking")]
        public async Task<IActionResult> ConfirmBanking(IFormFile file)
        {
            var allowExt = new string[] { "png", "jpg", "jpeg" };
            var ext = file.FileName.Split('.').LastOrDefault();
            if (!allowExt.Contains(ext))
            {
                throw new BadRequestException("File không hỗ trợ");
            }
            if(file.Length > 5 * 1024 * 1024)
            {
                throw new BadRequestException("File không được lớn hơn 5MB");
            }
            using(var mem = new MemoryStream())
            {
                await file.CopyToAsync(mem);
                var fileName = Guid.NewGuid().ToString() + "." + ext;
                var path = this.Env.ContentRootPath + "\\image-upload\\" + fileName;
                await System.IO.File.WriteAllBytesAsync(path, mem.ToArray());
                var host = this.Request.Scheme + "://" + this.Request.Host + "/" + path;
                return Response(host.Replace("\\", "/");
            }
        }
    }
}
