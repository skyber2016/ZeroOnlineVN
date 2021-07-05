using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Entity.DTO.Posts.Responses
{

    public class PostRenderResponse
    {
        public List<PostsEntity> Events { get; set; }
        public List<PostsEntity> Guilds { get; set; }
        public List<PostsEntity> Activities { get; set; }
        public List<PostsEntity> Images { get; set; }
        public List<PostsEntity> Banners { get; set; }
        public List<PostsEntity> Alls { get; set; }
    }

    public class PostGetResponse
    {
        [JsonIgnore]
        public DateTime PostModified { get; set; }

        [JsonProperty("title")]
        public string PostTitle { get; set; }

        [JsonIgnore]
        public string PostName { get; set; }

        [JsonProperty("url")]
        public string Url
        {
            get
            {
                return $"/chi-tiet/{this.PostName}";
            }
        }

        [JsonProperty("data")]
        public string PostTime
        {
            get
            {
                return this.PostModified.ToString("dd/MM");
            }
        }
    }
}