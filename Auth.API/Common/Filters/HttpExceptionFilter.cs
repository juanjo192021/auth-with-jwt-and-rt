using Auth.API.Common.Constants;
using Auth.API.Common.Responses;
using Auth.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.RegularExpressions;

namespace Auth.API.Common.Filters
{
    public class HttpExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HttpExceptionFilter> _logger;

        public HttpExceptionFilter(ILogger<HttpExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var errorResponse = new ErrorResponse
            {
                TraceId = context.HttpContext.TraceIdentifier
            };

            switch (exception)
            {
                case BadRequestException ex:
                    errorResponse.Type = ErrorTypeUris.BadRequest;
                    errorResponse.Title = "Bad Request";
                    errorResponse.Status = (int)HttpStatusCode.BadRequest;
                    errorResponse.Errors = ex.Message;
                    break;
                case UnauthorizedException ex:
                    errorResponse.Type = ErrorTypeUris.Unauthorized;
                    errorResponse.Title = "Unauthorized";
                    errorResponse.Status = (int)HttpStatusCode.Unauthorized;
                    errorResponse.Errors = ex.Message;
                    break;
                case ForbiddenException ex:
                    errorResponse.Type = ErrorTypeUris.Forbidden;
                    errorResponse.Title = "Not Found";
                    errorResponse.Status = (int)HttpStatusCode.NotFound;
                    errorResponse.Errors = ex.Message;
                    break;
                case NotFoundException ex:
                    errorResponse.Type = ErrorTypeUris.NotFound;
                    errorResponse.Title = "Not Found";
                    errorResponse.Status = (int)HttpStatusCode.NotFound;
                    errorResponse.Errors = ex.Message;
                    break;
                case ConflictException ex:
                    errorResponse.Type = ErrorTypeUris.Conflict;
                    errorResponse.Title = "Conflict";
                    errorResponse.Status = (int)HttpStatusCode.Conflict;
                    errorResponse.Errors = ex.Message;
                    break;
                case InternalServerErrorException ex:
                    errorResponse.Type = ErrorTypeUris.InternalServerError;
                    errorResponse.Title = "Internal Server Error";
                    errorResponse.Status = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Errors = ex.Message;
                    break;
                // ⚠️ Manejo especial para errores de base de datos (violación de UNIQUE KEY, FK, etc.)
                case DbUpdateException dbEx:
                    HandleDbUpdateException(dbEx, errorResponse);
                    break;
                default:
                    errorResponse.Type = exception.GetType().Name;
                    errorResponse.Title = "Unhandled Exception";
                    errorResponse.Status = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Errors = "Ocurrió un error inesperado. Contacte al administrador.";
                    break;
            }

            _logger.LogError(exception, "❌ [{Type}] {Message}", errorResponse.Type, errorResponse.Errors);

            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = errorResponse.Status
            };

            context.ExceptionHandled = true;
        }

        /// <summary>
        /// Maneja específicamente errores de base de datos (como claves duplicadas o FK violations)
        /// </summary>
        private void HandleDbUpdateException(DbUpdateException ex, ErrorResponse errorResponse)
        {
            // Si el InnerException es SqlException, analizamos el mensaje
            var sqlMessage = ex.InnerException?.Message ?? ex.Message;

            if (sqlMessage.Contains("UNIQUE KEY", StringComparison.OrdinalIgnoreCase))
            {
                errorResponse.Type = ErrorTypeUris.Conflict;
                errorResponse.Title = "Registro duplicado";
                errorResponse.Status = (int)HttpStatusCode.Conflict;

                // Opcional: limpiar el mensaje para hacerlo más amigable
                var match = Regex.Match(sqlMessage, @"clave duplicada.*\((.*?)\)", RegexOptions.IgnoreCase);
                var duplicatedValue = match.Success ? match.Groups[1].Value : "valor existente";

                errorResponse.Errors = $"Ya existe un registro con el valor '{duplicatedValue}'.";
            }
            else
            {
                // Otros errores de base de datos
                errorResponse.Type = ErrorTypeUris.InternalServerError;
                errorResponse.Title = "Error de base de datos";
                errorResponse.Status = (int)HttpStatusCode.InternalServerError;
                errorResponse.Errors = "Ocurrió un error al guardar los datos.";
            }
        }
    }
}
