using CORE_API.Cores;
using CORE_API.Helpers;
using NEWS_API.Cores.Exceptions;
using NEWS_API.DTO.Error.Responses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using System;

namespace NEWS_API.Attributes
{
    public class ErrorHandlingAttribute : ExceptionFilterAttribute
    {
        private ILoggerManager Logger { get; set; }
        private IHostEnvironment Env { get; set; }
        public ErrorHandlingAttribute(IUnitOfWork unitOfWork)
        {
            this.Logger = unitOfWork.Logger;
            this.Env = unitOfWork.GetEnvironment();
        }
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            if (exception is NotFoundException)
            {
                context.Result = new NotFoundResult();
            } 
            else if(exception is BadRequestException)
            {
                context.Result = new JsonResult(new Error400Response
                {
                    Message = exception.Message
                })
                { 
                    StatusCode = StatusCodes.Status400BadRequest
                };
            } 
            else if(exception is UnAuthorizeException)
            {
                context.Result = new UnauthorizedResult();
            } 
            else if(exception is ForbidenException)
            {
                context.Result = new ForbidResult();
            }
            else if(exception is RedirectChangePwdException)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status406NotAcceptable);
            }
            else
            {
                Logging(exception);
                var message = "Hệ thống hiện tại đang lỗi, vui lòng thực hiện lại sau";
                if (this.Env.IsDevelopment())
                {
                    message = exception.Message;
                }
                context.Result = new JsonResult(new Error400Response
                {
                    Message = message
                })
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
                Logger.Error(exception.StackTrace);
            }
        }
        private void Logging(Exception ex)
        {
            if(ex.InnerException != null)
            {
                Logging(ex.InnerException);
            }
            Logger.Error(ex.Message);
            Logger.Error(ex.StackTrace);
        }
    }
}
