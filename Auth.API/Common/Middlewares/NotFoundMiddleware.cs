using Auth.API.Common.Constants;
using Auth.API.Common.Responses;
using System.Net;

namespace Auth.API.Common.Middlewares
{
    public class NotFoundMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<NotFoundMiddleware> _logger;

        public NotFoundMiddleware(RequestDelegate next, ILogger<NotFoundMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            // Solo interceptar si la respuesta fue 404 y aún no se escribió nada
            if (context.Response.StatusCode == (int)HttpStatusCode.NotFound && !context.Response.HasStarted)
            {
                _logger.LogWarning("Ruta no encontrada: {Path}", context.Request.Path);

                var response = new ErrorResponse
                {
                    Type = ErrorTypeUris.NotFound,
                    Title = "Not Found",
                    Status = (int)HttpStatusCode.NotFound,
                    Errors = "El recurso solicitado no existe o la ruta es incorrecta.",
                    TraceId = context.TraceIdentifier
                };

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(response, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });
            }
        }
    }
}
