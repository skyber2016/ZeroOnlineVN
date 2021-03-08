using Forum_API.DTO.Base;

namespace Forum_API.DTO.ShoutBox.Responses
{
    public class ShoutBoxUpdateResponse
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public BaseUserGetResponse CreatedByUser { get; set; }
    }
}
