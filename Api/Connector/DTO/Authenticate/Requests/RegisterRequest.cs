using Forum_API.Cores.Validations;

namespace Forum_API.DTO.Authenticate.Requests
{
    public class RegisterRequest
    {
        [MaxLength(12)]
        public string Username { get; set; }
        [Length(6,12)]
        public string Password { get; set; }
        [IsEmail]
        public string Email { get; set; }
        [MaxLength(30)]
        public string FullName { get; set; }
    }
}
