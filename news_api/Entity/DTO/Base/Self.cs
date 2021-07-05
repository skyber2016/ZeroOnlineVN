using Newtonsoft.Json; 
namespace Entity.DTO.Base
{ 

    public class Self
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }

}