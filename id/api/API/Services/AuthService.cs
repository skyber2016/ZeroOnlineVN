using API.Common;
using API.Cores;
using API.Cores.Exceptions;
using API.DTO.Authenticate.Requests;
using API.DTO.Authenticate.Responses;
using API.Entities;
using API.Helpers;
using API.Security;
using API.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using SqlKata.Execution;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace API.Services.Authenticate
{
    public class AuthService : IAuthService
    {
        [Dependency]
        public IJwtService JwtService { get; set; }
        [Dependency]
        public IGeneralService<AccountEntity> AccountService { get; set; }
        [Dependency]
        public IGeneralService<RefreshTokenEntity> RefreshTokenService { get; set; }
        [Dependency]
        public IGeneralService<BotMessageEntity> BotMessageService { get; set; }
        [Dependency]
        public IMapper Mapper { get; set; }
        [Dependency]
        public IHttpContextAccessor Context { get; set; }
        [Dependency]
        public IMemoryCache Cache { get; set; }
        [Dependency]
        public IUnitOfWork UnitOfWork { get; set; }
        [Dependency]
        public IGeneralService<UserEntity> UserEntService { get; set; }
        [Dependency]
        public IUserService UserService { get; set; }

        public async Task<UserEntity> GetCQUser(IDbTransaction trans = null)
        {
            var currentUser = this.UserService.GetCurrentUser();
            var user = await this.UserEntService.SingleBy(new { account_id = currentUser.Id }, trans);
            return user;
        }
        public async Task<LoginResponse> Authenticate(string username, string password)
        {
            var user = await AccountService.SingleBy(new { name = username, password = MD5Helper.HashPassword(password) });
            if (user == null)
            {
                throw new BadRequestException(MessageConstants.GetMessage(Message.UserNotMatch));
            }
            LoginResponse response = new LoginResponse();
            await this.UnitOfWork.CreateTransaction(async tran =>
            {
                var userPrincipal = Mapper.Map<UserPrincipal>(user);
                var jwt = JwtService.GenerateJwtToken(userPrincipal);
                var refreshToken = JwtService.GenerateRefreshToken();
                refreshToken.UserId = user.Id.Value;
                var rfTokens = (await RefreshTokenService.FindBy(new { user_id = user.Id }).GetAsync<RefreshTokenEntity>(tran));
                foreach (var rfToken in rfTokens)
                {
                    if (!rfToken.IsActive)
                    {
                        continue;
                    }
                    rfToken.Revoked = DateTime.Now;
                    rfToken.RevokedByIP = Context.HttpContext.Connection.RemoteIpAddress.ToString();
                    await RefreshTokenService.UpdateAsync(rfToken, tran);
                }
                refreshToken = await this.RefreshTokenService.AddAsync(refreshToken, tran);

                response = new LoginResponse
                {
                    RefreshToken = refreshToken.Token,
                    Token = jwt,
                    Username = username,
                    FullName = user.Username,
                    P = this.UserService.IsAdmin(user.Username) ? AuthConstant.ADMIN : AuthConstant.NON_ADMIN
            };
                Cache.Set("JWT:" + user.Id, jwt, DateTime.Now.AddMinutes(UnitOfWork.JwtSetting.Value.Expire));
            });
            
            return response;
        }

        public async Task<AccountEntity> Register(RegisterRequest request)
        {
            var user = await AccountService.SingleBy(new { name = request.Username }); 
            if (user != null)
            {
                throw new BadRequestException(MessageConstants.GetMessage(Message.UserNameExist));
            }
            user = Mapper.Map<AccountEntity>(request);
            user.NetbarIP = this.Context.HttpContext.Connection.RemoteIpAddress.ToString();
            user.IPMask = this.Context.HttpContext.Connection.RemoteIpAddress.ToString();
            user.VIP = this.UnitOfWork.AppSettings.Value.VIPDefault;
            user.Password = MD5Helper.HashPassword(request.Password);
            user.Answer = request.Answer.Base64Encode();
            await this.UnitOfWork.CreateTransaction(async tran =>
            {
                user = await this.AccountService.AddAsync(user, tran);
                var builder = new StringBuilder();
                builder.AppendLine(string.Empty);
                builder.AppendLine($"--------------------**ĐĂNG KÝ TÀI KHOẢN**--------------------");
                builder.AppendLine($"**Tài khoản**: {request.Username}");
                builder.AppendLine($"**Số điện thoại**: {request.Sdt}");
                builder.AppendLine($"**Câu hỏi**: {CommonContants.Question[request.Question.Value]}");
                builder.AppendLine($"**Câu trả lời**: {request.Answer}");
                builder.AppendLine($"**Tạo tài khoản lúc**: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
                await this.BotMessageService.AddAsync(new BotMessageEntity
                {
                    Channel = ChannelConstant.REGISTER.ToString(),
                    Message = builder.ToString().Base64Encode()
                }, tran);
            });
            return user;
        }

        public async Task<string> RefreshToken(string refreshToken)
        {
            var token = await RefreshTokenService.SingleBy(new { Token = refreshToken });
            if (token == null)
            {
                throw new UnAuthorizeException();
            }
            if (!token.IsActive)
            {
                throw new UnAuthorizeException();
            }
            var user = await AccountService.SingleBy(new { id = token.UserId });
            var userPrincipal = Mapper.Map<UserPrincipal>(user);

            var jwt = JwtService.GenerateJwtToken(userPrincipal);
            Cache.Set("JWT:" + user.Id, jwt, DateTime.Now.AddMinutes(UnitOfWork.JwtSetting.Value.Expire));

            return jwt;
        }

        public async Task RevokeToken(int userId)
        {
            var refreshToken = (await RefreshTokenService.FindBy(new { user_id = userId }).GetAsync<RefreshTokenEntity>()).Where(x=>x.IsActive);
            foreach (var token in refreshToken)
            {
                token.Revoked = DateTime.Now;
                token.RevokedByIP = Context.HttpContext.Connection.RemoteIpAddress.ToString();
                await RefreshTokenService.UpdateAsync(token);
            }
            Cache.Remove("JWT:" + userId);
        }
    }
}
