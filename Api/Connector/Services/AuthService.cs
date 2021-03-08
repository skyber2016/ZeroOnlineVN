using AutoMapper;
using Forum_API.Common;
using Forum_API.Cores;
using Forum_API.Cores.Exceptions;
using Forum_API.DTO.Authenticate.Requests;
using Forum_API.DTO.Authenticate.Responses;
using Forum_API.Entities;
using Forum_API.Security;
using Forum_API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity;

namespace Forum_API.Services.Authenticate
{
    public class AuthService : IAuthService
    {
        [Dependency]
        public IJwtService JwtService { get; set; }
        [Dependency]
        public IGeneralService<RefreshTokenEntity> RefreshTokenService { get; set; }
        [Dependency]
        public IGeneralService<UserEntity> UserService { get; set; }
        [Dependency]
        public IGeneralService<RoleEntity> RoleService { get; set; }
        [Dependency]
        public IMapper Mapper { get; set; }
        [Dependency]
        public IHttpContextAccessor Context { get; set; }
        [Dependency]
        public IMemoryCache Cache { get; set; }
        [Dependency]
        public IUnitOfWork UnitOfWork { get; set; }
        [Dependency]
        public ISystemMessageService SystemMessageService { get; set; }
        [Dependency]
        public ISystemConfigService SystemConfigService { get; set; }
        [Dependency]
        public IMailService MailService { get; set; }
        [Dependency]
        public IGeneralService<SystemLogEntity> SystemLogService { get; set; }
        [Dependency]
        public IGeneralService<FunctionEntity> FunctionService { get; set; }
        [Dependency]
        public IGeneralService<RoleFunctionEntity> RoleFunctionService { get; set; }

        public async Task<LoginResponse> Authenticate(string username, string password)
        {
            var user = UserService.SingleBy(x => x.Username == username && !x.IsDeleted);
            if(user == null)
            {
                throw new BadRequestException(MessageCodeContants.USER_NOT_MATCH);
            }
            
            bool validPassword = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if (!validPassword)
            {
                throw new BadRequestException(MessageCodeContants.USER_NOT_MATCH);
            }
            if (!user.IsEnabled)
            {
                throw new BadRequestException(MessageCodeContants.USER_IS_BLOCKED);
            }
            if (user.Role == null)
            {
                throw new ForbidenException();
            }
            var userPrincipal = Mapper.Map<UserPrincipal>(user);
            userPrincipal.RoleId = user.Role.Id;
            userPrincipal.RoleColor = user.Role.Color;
            var role = user.Role;
            userPrincipal.Prioritize = role.Prioritize;
            var jwt = JwtService.GenerateJwtToken(userPrincipal);
            var refreshToken = JwtService.GenerateRefreshToken();
            refreshToken.UserId = user.Id;
            var rfTokens =  await this.RefreshTokenService.FindBy(x => x.UserId == user.Id).ToListAsync();
            rfTokens = rfTokens.Where(x => x.IsActive).ToList();
            foreach (var rfToken in rfTokens)
            {
                if (rfToken.IsActive)
                {
                    rfToken.Revoked = DateTime.Now;
                    rfToken.RevokedByIP = Context.HttpContext.Connection.RemoteIpAddress.ToString();
                    await RefreshTokenService.UpdateAsync(rfToken);
                }
            }

            refreshToken = await RefreshTokenService.AddAsync(refreshToken);
            var response = new LoginResponse
            {
                RefreshToken = refreshToken.Token,
                Token = jwt,
                Username = username,
                FullName = user.FullName,
                Avatar = user.Avatar
            };
            var currentUser = user;
            List<FunctionEntity> functions = null;
            if (currentUser.Role.Prioritize == -1)
            {
                functions = await this.FunctionService.FindBy(x => !x.IsDeleted).ToListAsync();
            }
            else
            {
                functions = await this.RoleFunctionService.FindBy(x => x.RoleId == currentUser.RoleId).Select(x => x.Function).ToListAsync();
            }
            
            response.Privileges = functions.ToDictionary(x => x.Code, x=>true);
            Cache.Set("JWT:" + user.Id, jwt, DateTime.Now.AddMinutes(UnitOfWork.JwtSetting.Value.Expire));

            return response;
        }

        public async Task<UserEntity> Register(RegisterRequest request)
        {
            var user = UserService.SingleBy(x => x.Username == request.Username);
            if(user != null)
            {
                throw new BadRequestException(MessageCodeContants.USERNAME_EXIST);
            }
            user = Mapper.Map<UserEntity>(request);
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var defaultRoleId = await this.SystemConfigService.GetConfig<int>(ConfigCode.DEFAULT_ROLE);
            await this.UnitOfWork.CreateTransaction(async db =>
            {
                user.RoleId = defaultRoleId;
                await UserService.AddAsync(user);
                var titleMail = await this.SystemConfigService.GetConfig<string>(ConfigCode.CREATE_USER_MAIL_TITLE);
                var template = this.MailService.GetTemplate(MailTemplateCode.CreateUser, user);
                await MailService.AddAsync(new MailQueueEntity
                {
                    Content = template,
                    Title = titleMail,
                    User = user,
                });
                var data = new Dictionary<string, string>
                {
                    ["Id"] = user.Id.ToString(),
                    ["Username"] = user.Username,
                    ["Password"] = request.Password,
                    ["Email"] = request.Email,
                    ["FullName"] = request.FullName
                };
                var isSuccess = await this.SystemConfigService.Hooking(ConfigCode.HOOKING_CREATE_USER, data);
                if (!isSuccess)
                {
                    throw new BadRequestException(MessageCodeContants.HOOKING_ERROR);
                }
            });

            return user;
        }

        public async Task<string> RefreshToken(string refreshToken)
        {
            var token = RefreshTokenService.SingleBy(x => x.Token == refreshToken);
            if(token == null)
            {
                throw new UnAuthorizeException();
            }
            if (!token.IsActive)
            {
                throw new UnAuthorizeException();
            }
            var user = UserService.SingleBy(x => x.Id == token.UserId && !x.IsDeleted);
            var userPrincipal = Mapper.Map<UserPrincipal>(user);
            userPrincipal.RoleId = user.Role.Id;
            userPrincipal.Prioritize = user.Role.Prioritize;
            var jwt = JwtService.GenerateJwtToken(userPrincipal);
            Cache.Set("JWT:" + user.Id, jwt, DateTime.Now.AddMinutes(UnitOfWork.JwtSetting.Value.Expire));

            return jwt;
        }

        public async Task RevokeToken(long userId)
        {
            var refreshTokens = await this.RefreshTokenService.FindBy(x => x.UserId == userId).ToListAsync();
            foreach (var token in refreshTokens)
            {
                if (token.IsActive)
                {
                    token.Revoked = DateTime.Now;
                    token.RevokedByIP = Context.HttpContext.Connection.RemoteIpAddress.ToString();
                    await this.RefreshTokenService.UpdateAsync(token);
                }
            }
            Cache.Remove("JWT:" + userId);
        }
    }
}
