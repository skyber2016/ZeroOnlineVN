using Newtonsoft.Json; 
namespace Entity.DTO.Base
{ 

    public class WpTerm
    {
        [JsonProperty("taxonomy")]
        public string Taxonomy { get; set; }

        [JsonProperty("embeddable")]
        public bool Embeddable { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }
    }

}