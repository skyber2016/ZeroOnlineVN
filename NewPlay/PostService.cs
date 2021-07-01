using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NewPlay
{
    public class Render
    {
        public string rendered { get; set; }
    }
    public class Post
    {
        public Render title { get; set; }
        public string slug { get; set; }
        public DateTime modified { get; set; }
    }
    public class PostService
    {
        private string Url = "https://caching.zeroonlinevn.com/wp-json/wp/v2/Posts?_embed=true&per_page=4";
        public List<Post> GetPosts()
        {
            try
            {
                using(var client = new WebClient())
                {
                    var content =  client.DownloadString(this.Url);
                    var json = JsonConvert.DeserializeObject<List<Post>>(content);
                    return json;
                }
            }
            catch (Exception)
            {
                return new List<Post>();
            }
            
        }
    }
}
