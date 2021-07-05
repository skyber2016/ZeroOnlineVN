using Newtonsoft.Json; 
namespace Entity.DTO.Base
{ 

    public class PredecessorVersion
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }
    }

}