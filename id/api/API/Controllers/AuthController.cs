using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using Unity;
using API.Attributes;
using API.Configurations;
using API.Cores;
using API.DTO.Authenticate.Requests;
using API.DTO.Authenticate.Responses;
using API.Services.Interfaces;
using API.DTO.User.Request;
using API.Entities;
using API.Cores.Exceptions;
using System;
using System.Linq;

namespace API.Controllers
{
    public class AuthController : BaseController
    {
        [Dependency]
        public IJwtService JwtService { get; set; }
        [Dependency]
        public IAuthService AuthService { get; set; }
        [Dependency]
        public IUserService UserService { get; set; }
        [Dependency]
        public IWebHostEnvironment env { get; set; }
        [Dependency]
        public IGeneralService<AccountEntity> AccountService { get; set; }

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(RegisterResponse))]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var user = await AuthService.Register(request);
            return Response<RegisterResponse>(user);
        }

        /// <summary>
        /// API Login
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(LoginResponse))]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var authen = await AuthService.Authenticate(request.Username, request.Password);
            if (this.env.IsDevelopment())
            {
                AutomationLoginMiddleware.LoginResponse = authen;
            }
            return Response(authen);
        }
        

        /// <summary>
        /// Khi đăng nhập hết hạn cần phải lấy token mới, nếu trả về lỗi 401 thì redirect login
        /// </summary>
        /// <param name="request">Refresh token: được cấp khi đăng nhập thành công</param>
        /// <returns></returns>
        [HttpPost]
        [Route("RefreshToken")]
        [AllowAnonymous]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(RefreshTokenResponse))]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
        {
            var newToken = await AuthService.RefreshToken(request.RefreshToken);
            return Response(new RefreshTokenResponse { 
                Token = newToken
            });
        }

        /// <summary>
        /// Khi đăng xuất sẽ xóa bỏ jwt hiện tại
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Logout")]
        [IgnoreRole]
        public async Task<IActionResult> Logout()
        {
            var userId = UserService.GetCurrentUser().Id;
            await AuthService.RevokeToken(userId);
            return Response();
        }


        [HttpPost]
        [Route("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var user = await this.AccountService.SingleBy(new
            {
                name = request.Username,
                Question1 = request.Question,
                Answer1 = request.Answer
            });
            if(user == null)
            {
                throw new BadRequestException("Tài khoản hoặc câu trả lời không khớp");
            }
            var newPassword = Guid.NewGuid().ToString().Split('-').LastOrDefault();
            user.Password = MD5Helper.HashPassword(newPassword);
            return Ok(new { password = newPassword });
        }
    }
}
