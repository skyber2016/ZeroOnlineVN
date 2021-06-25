using API.Common;
using API.Cores.Validations;

namespace API.DTO.Authenticate.Requests
{
    public class LoginRequest
    {
        [NotNull(Message.NotNull, "username")]
        public string Username { get; set; }
        [NotNull(Message.NotNull, "mật khẩu")]
        [MaxLength(30, Message.MaxLength, "mật khẩu")]
        public string Password { get; set; }
    }
}
