using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private Services.ILogService _logService;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, Services.ILogService logService)
        {
            this._logService = logService;
            try
            {
                _logService.LogDebug("Invoked");

                await _next(httpContext).ConfigureAwait(false);                

                if (httpContext.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    await HandleUnAuthorizeAsync(httpContext).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex).ConfigureAwait(false);
            }
        }

        private Task HandleUnAuthorizeAsync(HttpContext context)
        {

            _logService.LogDebug($"UnAuthorize Request");

            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }


            var model = new 
            {
                StatusCode = context.Response.StatusCode,
                Message = "Unauthorized attempt"
            };

            return context.Response.WriteAsync(model.Serialize());
        }

        

        

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logService.LogError($"Something went wrong", exception);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var model = new 
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error"
            };
            return context.Response.WriteAsync(model.Serialize());
        }
    }
}
