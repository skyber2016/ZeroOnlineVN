using API.Common;
using API.Cores.Validations;

namespace API.DTO.Authenticate.Requests
{
    public class RegisterRequest
    {
        [NotNull(Message.NotNull, "tên tài khoản")]
        [MinLength(4, Message.MinLength, "tên tài khoản")]
        [MaxLength(12, Message.MaxLength, "tên tài khoản")]
        public string Username { get; set; }

        [NotNull(Message.NotNull, "mật khẩu")]
        [MinLength(6, Message.MinLength, "mật khẩu")]
        [MaxLength(12, Message.MaxLength, "mật khẩu")]
        public string Password { get; set; }

        [IsEmail(Message.EmailInvalid)]
        public string Email { get; set; }

        [NotNull(Message.NotNull, "câu hỏi")]
        public int? Question { get; set; }

        [NotNull(Message.NotNull, "câu trả lời")]
        [MinLength(6, Message.MinLength, "câu trả lời")]
        [MaxLength(50, Message.MaxLength, "câu trả lời")]
        public string Answer { get; set; }

        [NotNull(Message.NotNull, "số điện thoại")]
        [MinLength(10, Message.MinLength, "số điện thoại")]
        [MaxLength(12, Message.MaxLength, "số điện thoại")]
        public string Sdt { get; set; }
    }
}
