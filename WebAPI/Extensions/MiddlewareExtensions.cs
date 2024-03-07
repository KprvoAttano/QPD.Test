using WebAPI.Extensions.Exceptions;
using WebAPI.Extensions.Logging;

namespace WebAPI.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void ConfigureCustomExceptionMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
        public static void ConfigureCustomLoggingMiddleware(this WebApplication app)
        {
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }
}
