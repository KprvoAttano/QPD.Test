using Microsoft.AspNetCore.Http;
using System.Text;

namespace WebAPI.Extensions.Logging
{
    public class RequestResponseLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestResponseLoggingMiddleware> logger)
    {
        public async Task Invoke(HttpContext context)
        {
            LogRequest(context);

            var originalBodyStream = context.Response.Body;
            try
            {

                using (var stream = new MemoryStream())
                {
                    context.Response.Body = stream;
                    await next(context);

                    LogResponse(context);

                    stream.Position = 0;

                    await stream.CopyToAsync(originalBodyStream);

                }
            }
            finally
            {
                context.Response.Body = originalBodyStream;
            }
        }

        private void LogRequest(HttpContext context)
        {
            var request = context.Request;

            var requestLog = new StringBuilder();
            requestLog.AppendLine("Incoming Request:");
            requestLog.AppendLine($"HTTP {request.Method} {request.Path}");
            requestLog.AppendLine($"Host: {request.Host}");
            requestLog.AppendLine($"Content-Type: {request.ContentType}");
            requestLog.AppendLine($"Content-Length: {request.ContentLength}");

            logger.LogInformation(requestLog.ToString());
        }

        private void LogResponse(HttpContext context)
        {
            var response = context.Response;

            var responseLog = new StringBuilder();
            responseLog.AppendLine("Outgoing Response:");
            responseLog.AppendLine($"HTTP {response.StatusCode}");
            responseLog.AppendLine($"Content-Type: {response.ContentType}");
            responseLog.AppendLine($"Content-Length: {response.ContentLength}");

            logger.LogInformation(responseLog.ToString());
        }
    }
}
