using System;

namespace API.DTO.Card.Respones
{
    public class CardGetResponse
    {
        public int Id { get; set; }
        public string Seri { get; set; }
        public string Code { get; set; }
        public string Amount { get; set; }
        public string Message { get; set; }
        public string TranId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
