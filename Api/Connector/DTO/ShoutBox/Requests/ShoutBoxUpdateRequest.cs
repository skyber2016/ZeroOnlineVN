using Forum_API.DTO.Base;

namespace Forum_API.DTO.ShoutBox.Requests
{
    public class ShoutBoxUpdateRequest : UpdateRequest
    {
        public string Message { get; set; }
    }
}
