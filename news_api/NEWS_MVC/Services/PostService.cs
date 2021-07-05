using Entity;
using Entity.DTO.Posts.Responses;
using Microsoft.EntityFrameworkCore;
using NEWS_MVC.Helpers;
using NEWS_MVC.Services.Interfaces;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Unity;

namespace NEWS_MVC.Services
{
    public class PostService : IPostService
    {
        [Dependency]
        public IGeneralService<PostsEntity> PostEntService { get; set; }
        [Dependency]
        public IGeneralService<OptionsEntity> OptionService { get; set; }

        private PostRenderResponse Response { get; set; }

        public PostRenderResponse GetPosts()
        {
            this.Response = MemoryCacheHelper.GetValue<PostRenderResponse>("PostRenderResponse");
            if (this.Response == null)
            {
                var sticky = OptionService.FindBy(x => x.OptionName == "sticky_posts").Select(x => x.OptionValue).FirstOrDefault();
                sticky = sticky ?? string.Empty;
                string pattern = @"\d+";
                var regex = Regex.Matches(sticky, pattern, RegexOptions.ECMAScript);
                var stickies = regex.Select(x => Convert.ToInt32(x.Value)).Skip(1).ToList();
                var slugs = new string[] { "su-kien", "hoat-dong", "huong-dan", "hinh-anh", "banner" };
                string SQL = $"SELECT p.*, t.slug, t.name FROM wp_posts p LEFT JOIN wp_term_relationships rel ON rel.object_id = p.ID LEFT JOIN wp_term_taxonomy tax ON tax.term_taxonomy_id = rel.term_taxonomy_id LEFT JOIN wp_terms t ON t.term_id = tax.term_id";
                var datas = PostEntService
                    .GetDbSet()
                    .FromSqlRaw(SQL)
                    .Where(x => slugs.Contains(x.Slug.ToLower()) && (x.PostStatus == "publish" || x.PostStatus == "inherit"))
                    .OrderByDescending(x => sticky.Contains(x.Id))
                    .ToList()
                    ;
                this.Response = new PostRenderResponse()
                {
                    Events = datas.Where(x => x.Slug.ToLower() == "su-kien" && x.PostType == "post").ToList(),
                    Activities = datas.Where(x => x.Slug.ToLower() == "hoat-dong" && x.PostType == "post").ToList(),
                    Alls = datas.Where(x => x.PostType == "post").ToList(),
                    Banners = datas.Where(x => x.Slug.ToLower() == "banner" && x.PostType == "attachment").ToList(),
                    Guilds = datas.Where(x => x.Slug.ToLower() == "huong-dan" && x.PostType == "post").ToList(),
                    Images = datas.Where(x => x.Slug.ToLower() == "hinh-anh" && x.PostType == "post").ToList()
                };
                MemoryCacheHelper.Add("PostRenderResponse", this.Response, DateTime.Now.AddHours(1));
            }

            return this.Response;
        }
    }
}
