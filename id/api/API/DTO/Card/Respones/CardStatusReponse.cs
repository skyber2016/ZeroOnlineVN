using System.Text.Json.Serialization;

namespace API.DTO.Card.Respones
{
    public class CardStatusReponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public string Status { get; set; }
    }
}
