using Entity.DTO.Posts.Responses;
using Microsoft.AspNetCore.Mvc;
using NEWS_MVC.Cores;
using NEWS_MVC.Services.Interfaces;
using System.Linq;
using Unity;

namespace NEWS_MVC.Controllers
{
    public class NewsController : BaseController
    {
        [Dependency]
        public IPostService PostService { get; set; }

        [Route("bai-viet")]
        public IActionResult Index()
        {
            var posts = PostService.GetPosts();
            this.ViewBag.alls = posts.Alls.Select(x => Mapper.Map<PostGetResponse>(x));
            this.ViewBag.events = posts.Events.Select(x => Mapper.Map<PostGetResponse>(x));
            this.ViewBag.activities = posts.Activities.Select(x => Mapper.Map<PostGetResponse>(x));
            this.ViewBag.guilds = posts.Guilds.Select(x => Mapper.Map<PostGetResponse>(x));
            return View();
        }
    }
}
