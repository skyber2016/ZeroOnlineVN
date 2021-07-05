using Newtonsoft.Json; 
namespace Entity.DTO.Base
{ 

    public class Reply
    {
        [JsonProperty("embeddable")]
        public bool Embeddable { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }
    }

}