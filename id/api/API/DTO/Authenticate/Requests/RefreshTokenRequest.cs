using System.ComponentModel.DataAnnotations;

namespace API.DTO.Authenticate.Requests
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
