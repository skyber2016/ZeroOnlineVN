using Forum_API.Common;
using Forum_API.Configurations;
using Forum_API.Cores;
using Forum_API.Cores.Exceptions;
using Forum_API.DTO.File.Responses;
using Forum_API.Entities;
using Forum_API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using Unity;

namespace Forum_API.Controllers
{
    public class UserController : BaseController
    {
        [Dependency]
        public IFileService FileService { get; set; }
        [Dependency]
        public IGeneralService<UserEntity> UserEntService { get; set; }
        [Dependency]
        public IOptions<SftpSetting> Setting { get; set; }

        private SftpSetting FtpSetting
        {
            get
            {
                return this.Setting.Value;
            }
        }

        [HttpPost]
        [Route("Avatar")]
        public async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            var allowExt = new string[] { ".png", ".jpg", ".jpeg", ".gif" };
            var fileExt = "." + file.FileName.ToLower().Split('.').LastOrDefault();
            if (!allowExt.Any(x => x == fileExt) || file.Length > 1 * 1024 * 1024)
            {
                throw new BadRequestException(MessageCodeContants.FILE_NOT_ALLOW);
            }

            var currentUser = this.UserService.GetCurrentUser();
            var user = this.UserEntService.SingleBy(x => x.Id == currentUser.Id);
            var physicalName = await FileService.Push(file);

            await this.UnitOfWork.CreateTransaction(async db =>
            {
                await FileService.AddAsync(new FileEntity
                {
                    FileName = file.FileName,
                    PhysicalName = physicalName,
                    Ext = fileExt,
                    Size = file.Length,
                    ContentType = file.ContentType
                });
                user.Avatar = FtpSetting.DomainDownload + physicalName;
                await this.UserEntService.UpdateAsync(user);
            });
            

            return Response(new FileGetResponse
            {
                Url = FtpSetting.DomainDownload + physicalName
            });
        }
    }
}
