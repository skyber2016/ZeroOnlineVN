using Newtonsoft.Json; 
namespace Entity.DTO.Base
{ 

    public class Guid
    {
        [JsonProperty("rendered")]
        public string Rendered { get; set; }
    }

}