using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PlayWebCore.Controllers
{
    public class Posts
    {
        public Text title { get; set; }
        public string slug { get; set; }
        public DateTime date { get; set; }
    }
    public class Text
    {
        public string rendered { get; set; }
    }
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment env;
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            this.env = env;
        }

        public async Task<IActionResult> Index(int version)
        {
            using(var http = new HttpClient())
            {
                var str = await http.GetAsync("http://caching.zeroonlinevn.com/wp-json/wp/v2/posts?page=1&per_page=3");
                var data = JsonConvert.DeserializeObject<List<Posts>>(await str.Content.ReadAsStringAsync());
                this.ViewBag.Posts = data;
                this.ViewBag.Version = version;
                var path = env.ContentRootPath + "\\version.dat";
                var svVersion = 0;
                if (System.IO.File.Exists(path))
                {
                    svVersion = Convert.ToInt32(System.IO.File.ReadAllText(path));
                }
                var pathBG = env.ContentRootPath + "\\wwwRoot\\bg.jpg";
                long versionBG = 0;
                if (System.IO.File.Exists(pathBG))
                {
                    versionBG = new System.IO.FileInfo(pathBG).LastWriteTime.Ticks;
                }
                this.ViewBag.ServerVersion = svVersion;
                this.ViewBag.versionBG = versionBG;
            }
            return View();
        }
        
    }
}
