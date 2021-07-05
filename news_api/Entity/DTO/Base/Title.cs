using Newtonsoft.Json; 
namespace Entity.DTO.Base
{ 

    public class Title
    {
        [JsonProperty("rendered")]
        public string Rendered { get; set; }
    }

}