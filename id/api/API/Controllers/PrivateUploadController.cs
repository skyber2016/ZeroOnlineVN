using API.Configurations;
using API.Cores;
using API.DTO.PrivateUpload.Requests;
using API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;
using Unity;

namespace API.Controllers
{
    public class PrivateUploadController : BaseController
    {
        [Dependency]
        public IOptions<AppSettings> Options { get; set; }
        [Dependency]
        public ILoggerManager Logger { get; set; }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Upload([FromForm]PrivateUploadCreateRequest request)
        {
            var files = Directory.GetFiles(this.Options.Value.PathSaveFile, "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                var day = (DateTime.Now - fileInfo.LastWriteTime).TotalDays;
                if (day > this.Options.Value.SaveByDays)
                {
                    System.IO.File.Delete(file);
                }
            }
            if (request.File == null) return Response();
            if(!request.IsValid())
            {
                Logger.Info($"Request invalid {request.SecretKey}");
                return Response();
            }
            if(!Directory.Exists(this.Options.Value.PathSaveFile))
            {
                Directory.CreateDirectory(this.Options.Value.PathSaveFile);
            }
            string filePath = Path.Combine(this.Options.Value.PathSaveFile, request.File.FileName);
            Logger.Info($"Save file path {filePath} with {request.File.Length.ToString("#,##0")} bytes");
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(fileStream);
            }
            return Response();
        }

    }
}
