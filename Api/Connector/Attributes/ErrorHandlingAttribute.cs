using Forum_API.Cores.Exceptions;
using Forum_API.DTO.Error.Responses;
using Forum_API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Forum_API.Attributes
{
    public class ErrorHandlingAttribute : ExceptionFilterAttribute
    {
        private ILoggerManager Logger { get; set; }
        public ErrorHandlingAttribute(ILoggerManager logger)
        {
            this.Logger = logger;
        }
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            Logging(exception);
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
            else
            {
                context.Result = new JsonResult(new Error400Response
                {
                    Message = "Hệ thống tạm thời ngưng phục vụ"
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
        }
    }
}
