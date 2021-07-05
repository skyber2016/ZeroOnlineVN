using Newtonsoft.Json; 
namespace Entity.DTO.Base
{ 

    public class WpAttachment
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }

}