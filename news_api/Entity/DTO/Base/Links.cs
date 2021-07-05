using Newtonsoft.Json; 
using System.Collections.Generic; 
namespace Entity.DTO.Base
{ 

    public class Links
    {
        [JsonProperty("self")]
        public List<Self> Self { get; set; }

        [JsonProperty("collection")]
        public List<Collection> Collection { get; set; }

        [JsonProperty("about")]
        public List<About> About { get; set; }

        [JsonProperty("author")]
        public List<Author> Author { get; set; }

        [JsonProperty("replies")]
        public List<Reply> Replies { get; set; }

        [JsonProperty("version-history")]
        public List<VersionHistory> VersionHistory { get; set; }

        [JsonProperty("predecessor-version")]
        public List<PredecessorVersion> PredecessorVersion { get; set; }

        [JsonProperty("wp:featuredmedia")]
        public List<WpFeaturedmedia> WpFeaturedmedia { get; set; }

        [JsonProperty("wp:attachment")]
        public List<WpAttachment> WpAttachment { get; set; }

        [JsonProperty("wp:term")]
        public List<WpTerm> WpTerm { get; set; }

        [JsonProperty("curies")]
        public List<Cury> Curies { get; set; }
    }

}