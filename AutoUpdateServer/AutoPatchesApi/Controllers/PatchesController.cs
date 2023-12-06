using AutoPatchesApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoPatchesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatchesController : ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public PatchesController(IHttpContextAccessor ctx)
        {
            _contextAccessor = ctx;
        }
        [HttpGet]
        public IEnumerable<VersionConfiguration> Get()
        {
            var json = System.IO.File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "patches.json"));
            return JsonConvert.DeserializeObject<List<VersionConfiguration>>(json);
        }


        [HttpGet("{fileName}")]
        public IActionResult Get(string fileName)
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var pathToFile = Path.Combine(basePath, "patches", fileName);
            if (!System.IO.File.Exists(pathToFile))
            {
                return NotFound();
            }
            var byteArray = System.IO.File.ReadAllBytes(pathToFile);
            return File(byteArray, "application/octet-stream", fileName, true);
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            var form = await _contextAccessor.HttpContext.Request.ReadFormAsync();
            var file = form.Files.FirstOrDefault();
            if(file == null)
            {
                return NotFound();
            }
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var pathJson = Path.Combine(basePath, "patches.json");
            var json = System.IO.File.ReadAllText(pathJson);
            var versions = JsonConvert.DeserializeObject<List<VersionConfiguration>>(json);
            var lastVersion = versions.OrderBy(x => x.Version).LastOrDefault()?.Version ?? 1000;
            var fileName = $"{lastVersion + 1}.exe";
            var dir = Path.Combine(basePath, "patches");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var pathToSave = Path.Combine(dir, fileName);
            versions.Add(new VersionConfiguration
            {
                Version = lastVersion + 1,
                File = fileName
            });
            using (var stream = file.OpenReadStream())
            {
                using (var mem = new MemoryStream())
                {
                    await stream.CopyToAsync(mem);
                    var byteArrays = mem.ToArray();
                    System.IO.File.WriteAllBytes(pathToSave, byteArrays);
                }
            }
            System.IO.File.WriteAllText(pathJson, JsonConvert.SerializeObject(versions, Formatting.Indented));
            return Ok($"Tạo bản cập nhật {fileName} thành công");
        }
    }
}
