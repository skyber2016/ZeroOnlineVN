namespace API.DTO.GiftCode.Requests
{
    public class GiftCodeCreateRequest
    {
        public string GiftCode { get; set; }
        public int Type { get; set; }
        public int ItemId { get; set; }
    }
}
