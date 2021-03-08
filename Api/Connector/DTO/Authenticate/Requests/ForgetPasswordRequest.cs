using Forum_API.Cores.Validations;

namespace Forum_API.DTO.Authenticate.Requests
{
    public class ForgetPasswordRequest
    {
        [NotNull("Please enter the Email")]
        [IsEmail("Incorrect email format")]
        public string Email { get; set; }
    }
}
