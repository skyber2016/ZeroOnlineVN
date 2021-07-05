using Newtonsoft.Json; 
namespace Entity.DTO.Base
{ 

    public class Content
    {
        [JsonProperty("rendered")]
        public string Rendered { get; set; }

        [JsonProperty("protected")]
        public bool Protected { get; set; }
    }

}