using Auth.Application.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Helpers
{
    public class ResponseHelper
    {
        //private readonly IMapper _mapper = mapper;

        // Mapea la respuesta de la paginación y retorna la respuesta mapeada

        //public PaginationResponse<List<TEntity>> CreatePaginationResponse<TEntity>(PaginationResponse<List<TEntity>> entities)
        //{
        //    return new PaginationResponse<List<TEntity>>
        //    {
        //        Page = entities.Page,
        //        PageSize = entities.PageSize,
        //        TotalRecords = entities.TotalRecords,
        //        TotalPages = entities.TotalPages,
        //        HasPreviousPage = entities.HasPreviousPage,
        //        HasNextPage = entities.HasNextPage,
        //        //Data = _mapper.Map<List<TDTO>>(entities.Data)
        //        Data = entities.Data
        //    };
        //}

        //// Creación, búsqueda y actualización la respuesta cuando todo salio sasticfactoriamente

        //public IActionResult CreateSuccessResponse<TEntity>(
        //    TEntity entities,
        //    int statusCode,
        //    string message,
        //    string actionName = null,
        //    string controllerName = null,
        //    object routeValues = null)
        //{
        //    var response = new DataDto<TEntity>
        //    {
        //        StatusCode = actionName != null ? StatusCodes.Status201Created : statusCode,
        //        Message = message,
        //        Success = true,
        //        Data = entities
        //    };

        //    if (!string.IsNullOrEmpty(actionName) && !string.IsNullOrEmpty(controllerName))
        //    {
        //        return new CreatedAtActionResult(actionName, controllerName, routeValues, response);
        //    }

        //    return new ObjectResult(response) { StatusCode = statusCode };
        //}

        //// Eliminación satisfactoria

        //public IActionResult CreateSuccessDeleteResponse<TEntity>(TEntity entities, string message)
        //{
        //    return new OkObjectResult(
        //        new DataDto<TEntity>
        //        {
        //            StatusCode = 200,
        //            Message = message,
        //            Success = true
        //        }
        //    );
        //}


    }
}
