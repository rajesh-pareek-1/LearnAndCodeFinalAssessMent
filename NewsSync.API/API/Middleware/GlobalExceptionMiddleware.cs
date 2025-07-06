using System.Net;
using System.Text.Json;

namespace NewsSync.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();
                _logger.LogError(ex, $"{errorId}: Unhandled exception occurred: {ex.Message}");

                var (statusCode, message) = MapExceptionToResponse(ex);

                context.Response.StatusCode = (int)statusCode;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    ErrorId = errorId,
                    Message = message,
                    ExceptionType = ex.GetType().Name
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }

        private (HttpStatusCode StatusCode, string Message) MapExceptionToResponse(Exception ex)
        {
            return ex switch
            {
                ArgumentNullException => (HttpStatusCode.BadRequest, ex.Message),
                ArgumentException => (HttpStatusCode.BadRequest, ex.Message),
                InvalidOperationException => (HttpStatusCode.BadRequest, ex.Message),
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized access"),
                KeyNotFoundException => (HttpStatusCode.NotFound, ex.Message),
                _ => (HttpStatusCode.InternalServerError, "Something went wrong. Please try again later.")
            };
        }
    }
}
