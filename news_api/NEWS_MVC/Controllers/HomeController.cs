using Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEWS_MVC.Cores;
using NEWS_MVC.Helpers;
using NEWS_MVC.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using Unity;

namespace NEWS_MVC.Controllers
{

    public class HomeController : BaseController
    {
        [Dependency]
        public IGeneralService<PostsEntity> PostService { get; set; }

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("trang-chu")]
        public IActionResult News()
        {
            return View();
        }


        [Route("chi-tiet/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            var post = MemoryCacheHelper.GetValue<PostsEntity>(slug);
            if(post == null)
            {
                post = await this.PostService
                .GetDbSet()
                .FromSqlRaw("SELECT p.*, 'slug' as slug, 'name' as name FROM wp_posts p")
                .Where(x => x.PostName.ToLower() == slug.ToLower() && x.PostStatus == "publish" && x.PostType == "post")
                .Include(x => x.Author)
                .FirstOrDefaultAsync();
            }
            if(post == null)
            {
                return NotFound();
            }
            else
            {
                MemoryCacheHelper.Add(slug, post);
            }
            var thumbnail = MemoryCacheHelper.GetValue<PostsEntity>(slug + "thumb_nail");
            if(thumbnail == null)
            {
                thumbnail = await this.PostService
                .GetDbSet()
                .FromSqlRaw($"SELECT p.*, 'slug' as slug, 'name' as name FROM wp_postmeta m  LEFT JOIN wp_posts p on p.ID = m.meta_value WHERE  m.meta_key = '_thumbnail_id' AND m.post_id = '{post.Id}'")
                .FirstOrDefaultAsync()
                ;
                if(thumbnail != null)
                {
                    MemoryCacheHelper.Add(slug + "thumb_nail", thumbnail);
                }
            }
            
            this.ViewBag.Post = post;
            this.ViewBag.Thumbnail = thumbnail?.Guid ?? "/images/thumbnail.jpg";
            return View();
        }

    }
}
