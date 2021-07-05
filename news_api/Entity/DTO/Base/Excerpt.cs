using Newtonsoft.Json; 
namespace Entity.DTO.Base
{ 

    public class Excerpt
    {
        [JsonProperty("rendered")]
        public string Rendered { get; set; }

        [JsonProperty("protected")]
        public bool Protected { get; set; }
    }

}