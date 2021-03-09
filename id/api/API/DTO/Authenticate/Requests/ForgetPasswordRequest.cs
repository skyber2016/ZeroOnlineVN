using API.Common;
using API.Cores.Validations;

namespace API.DTO.Authenticate.Requests
{
    public class ForgetPasswordRequest
    {
        [NotNull(Message.NotNull, "Email")]
        [IsEmail(Message.EmailInvalid)]
        public string Email { get; set; }
    }
}
