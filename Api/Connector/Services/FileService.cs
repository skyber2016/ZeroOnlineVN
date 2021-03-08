using Forum_API.Configurations;
using Forum_API.Entities;
using Forum_API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Renci.SshNet;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Unity;

namespace Forum_API.Services
{
    public class FileService : GeneralService<FileEntity>, IFileService
    {
        [Dependency]
        public IOptions<SftpSetting> Setting { get; set; }

        private SftpSetting FtpSetting
        {
            get
            {
                return this.Setting.Value;
            }
        }
        public async Task Connect(Func<SftpClient, Task> action)
        {
            using (SftpClient sftp = new SftpClient(FtpSetting.Host, FtpSetting.Username, FtpSetting.Password))
            {
                sftp.Connect();
                await action(sftp);
                sftp.Disconnect();
            }
        }
        private string GetExt(string fileName)
        {
            return "." + fileName.Split('.').LastOrDefault();
        }
        public async Task<string> Push(IFormFile file)
        {
            var physicalName = Guid.NewGuid().ToString();
            string url = "";
            await this.Connect(async sftp =>
            {
                using (var stream = file.OpenReadStream())
                {
                    sftp.BufferSize = (uint)file.Length; // bypass Payload error large files
                    url = this.FtpSetting.Root + physicalName + GetExt(file.FileName);
                    sftp.UploadFile(stream, url);
                }
            });
            return url;
        }
    }
}
