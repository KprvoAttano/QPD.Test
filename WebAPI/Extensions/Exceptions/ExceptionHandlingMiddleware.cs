using Domain.Models.Errors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;

namespace WebAPI.Extensions.Exceptions
{
    public class ExceptionHandlingMiddleware(
        RequestDelegate requestDelegate,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        public RequestDelegate RequestDelegate = requestDelegate;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await RequestDelegate(context);
            }
            catch (Exception ex)
            {
                logger.LogError("Something went wrong: {0}", ex);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            logger.LogError(ex.ToString());

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error from the custom middleware."
            }.ToString()).ConfigureAwait(false);
        }
    }
}
