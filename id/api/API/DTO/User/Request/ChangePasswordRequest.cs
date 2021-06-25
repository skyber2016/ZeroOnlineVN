using API.Cores.Validations;

namespace API.DTO.User.Request
{
    public class ChangePasswordRequest
    {
        [NotNull("Mật khẩu không được bỏ trống")]
        public string OldPassword { get; set; }
        [NotNull("Mật khẩu mới không được bỏ trống")]
        public string NewPassword { get; set; }
    }
}
