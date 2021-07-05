using Newtonsoft.Json;
namespace Entity.DTO.Base
{

    public class About
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }

}