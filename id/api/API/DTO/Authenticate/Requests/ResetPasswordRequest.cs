using API.Cores.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO.User.Request
{
    public class ResetPasswordRequest
    {
        [NotNull(Common.Message.NotNull, "tên tài khoản")]
        public string Username { get; set; }

        public int Question { get; set; }

        [NotNull(Common.Message.NotNull, "câu trả lời")]
        public string Answer { get; set; }
    }
}
