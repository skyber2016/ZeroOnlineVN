using Forum_API.Cores.Validations;

namespace Forum_API.DTO.Authenticate.Requests
{
    public class LoginRequest
    {
        [NotNull]
        public string Username { get; set; }
        [NotNull]
        [Length(6,12)]
        public string Password { get; set; }
    }
}
