using Forum_API.Cores.Validations;

namespace Forum_API.DTO.ShoutBox.Requests
{
    public class ShoutBoxCreateRequest
    {
        [MaxLength(150)]
        public string Message { get; set; }
    }
}
