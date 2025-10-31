using System.Net;

namespace Auth.API.Common.Middlewares
{
    public class NotFoundMiddleware
    {
        private readonly RequestDelegate _next;

        public NotFoundMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            // Si llega aquí y el status es 404 => no se encontró el recurso
            if (context.Response.StatusCode == (int)HttpStatusCode.NotFound && !context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";
                var response = new
                {
                    statusCode = 404,
                    message = "El recurso solicitado no existe o la ruta es incorrecta."
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
