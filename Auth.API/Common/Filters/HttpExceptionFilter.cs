using Auth.API.Common.Constants;
using Auth.API.Common.Responses;
using Auth.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

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
    }
}
