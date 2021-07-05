using Newtonsoft.Json; 
namespace Entity.DTO.Base
{ 

    public class VersionHistory
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }
    }

}