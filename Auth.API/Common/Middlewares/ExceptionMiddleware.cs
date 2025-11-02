using Auth.API.Common.Constants;
using Auth.API.Common.Responses;
using System.Net;
using System.Text.Json;

namespace Auth.API.Common.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "❌ Unhandled exception occurred");

            var statusCode = exception switch
            {
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                ArgumentException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };

            var errorResponse = new ErrorResponse
            {
                Type = ErrorTypeUris.InternalServerError,
                Title = statusCode switch
                {
                    HttpStatusCode.BadRequest => "Bad Request",
                    HttpStatusCode.Unauthorized => "Unauthorized",
                    _ => "Internal Server Error"
                },
                Status = (int)statusCode,
                Errors = exception.Message,
                TraceId = context.TraceIdentifier
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = errorResponse.Status;

            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, options));
        }
    }
}
