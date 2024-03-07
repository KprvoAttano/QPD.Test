using System.Text;

namespace WebAPI.Extensions.Logging
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next,
            ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            LogRequest(context);

            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                await _next(context);

                context.Response.Body = responseBody;

                LogResponse(context);

                await responseBody.CopyToAsync(originalBodyStream);
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

            _logger.LogInformation(requestLog.ToString());
        }

        private void LogResponse(HttpContext context)
        {
            var response = context.Response;

            var responseLog = new StringBuilder();
            responseLog.AppendLine("Outgoing Response:");
            responseLog.AppendLine($"HTTP {response.StatusCode}");
            responseLog.AppendLine($"Content-Type: {response.ContentType}");
            responseLog.AppendLine($"Content-Length: {response.ContentLength}");

            _logger.LogInformation(responseLog.ToString());
        }
    }
}
