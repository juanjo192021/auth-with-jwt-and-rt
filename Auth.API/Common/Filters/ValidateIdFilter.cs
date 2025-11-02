using Auth.API.Common.Constants;
using Auth.API.Common.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Auth.API.Common.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateIdFilterAttribute : ActionFilterAttribute
    {
        private readonly string _parameterName;

        public ValidateIdFilterAttribute(string parameterName = "id")
        {
            _parameterName = parameterName;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue(_parameterName, out var value))
            {
                if (value is int id)
                {
                    if (id <= 0)
                    {
                        context.Result = new BadRequestObjectResult(new ErrorResponse
                        {
                            Type = ErrorTypeUris.BadRequest,
                            Title = "BadRequest",
                            Status = 400,
                            Errors = "El Id debe ser un número mayor que cero.",
                            TraceId = context.HttpContext.TraceIdentifier
                        });
                    }
                }
                else
                {
                    context.Result = new BadRequestObjectResult(new ErrorResponse
                    {
                        Type = ErrorTypeUris.BadRequest,
                        Title = "BadRequest",
                        Status = 400,
                        Errors = "El Id debe ser un número válido.",
                        TraceId = context.HttpContext.TraceIdentifier
                    });
                }
            }
            else
            {
                context.Result = new BadRequestObjectResult(new ErrorResponse
                {
                    Type = ErrorTypeUris.BadRequest,
                    Title = "BadRequest",
                    Status = 400,
                    Errors = $"No se encontró el parámetro '{_parameterName}'.",
                    TraceId = context.HttpContext.TraceIdentifier
                });
            }
        }
    }
}
