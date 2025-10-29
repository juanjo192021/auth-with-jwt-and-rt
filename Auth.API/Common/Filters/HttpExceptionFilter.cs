using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Auth.Application.Common.Exceptions;

namespace Auth.API.Common.Filters
{
    public class HttpExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case BadRequestException ex:
                    context.Result = new BadRequestObjectResult(new { message = ex.Message });
                    break;
                case UnauthorizedException ex:
                    context.Result = new UnauthorizedObjectResult(new { message = ex.Message });
                    break;
                case NotFoundException ex:
                    context.Result = new NotFoundObjectResult(new { message = ex.Message });
                    break;
                case ConflictException ex:
                    context.Result = new ObjectResult(new { message = ex.Message })
                    {
                        StatusCode = 409
                    };
                    break;
                case InternalServerErrorException ex:
                    context.Result = new ObjectResult(new { message = ex.Message })
                    {
                        StatusCode = 500
                    };
                    break;
                default:
                    context.Result = new ObjectResult(new { message = "Ocurrió un error interno por default" })
                    {
                        StatusCode = 500
                    };
                    break;
            }

            context.ExceptionHandled = true;
        }
    }
}
