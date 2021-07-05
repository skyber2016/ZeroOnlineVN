using Newtonsoft.Json; 
namespace Entity.DTO.Base
{ 

    public class Collection
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }

}