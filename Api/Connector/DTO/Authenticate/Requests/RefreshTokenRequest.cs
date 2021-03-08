using System.ComponentModel.DataAnnotations;

namespace Forum_API.DTO.Authenticate.Requests
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
