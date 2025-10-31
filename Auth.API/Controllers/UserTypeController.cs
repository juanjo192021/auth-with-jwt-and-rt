using Auth.API.Common.Requests;
using Auth.API.Common.Responses;
using Auth.Application.Common.Dtos;
using Auth.API.Common.Filters;
using Auth.Application.DTOs.UserType;
using Auth.Application.UseCases.UserTypeCases;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    [Route("user-types")]
    [ApiController]
    [Authorize]
    public class UserTypeController : Controller
    {
        private readonly FindAllUseCase _findAllUseCase;
        private readonly FindByIdUseCase _findByIdUseCase;
        private readonly IMapper _mapper;

        public UserTypeController(FindAllUseCase findAllUseCase, FindByIdUseCase findByIdUseCase, IMapper mapper)
        {
            _findAllUseCase = findAllUseCase;
            _findByIdUseCase = findByIdUseCase;
            _mapper = mapper;
        }
        
        [HttpGet]
        [Authorize(Roles = "ManageUsers")]
        public async Task<IActionResult> FindAll([FromQuery] PaginationRequest paginationRequest)
        {
            var paginationDto = _mapper.Map<PaginationDto>(paginationRequest);
            var userTypes = await _findAllUseCase.FindAll(paginationDto);
            var response = _mapper.Map<PaginationResponse<List<UserTypeDto>>>(userTypes);
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        [ValidateIdFilterAttribute]
        public async Task<IActionResult> FindOne(int id)
        { 
            var userType = await _findByIdUseCase.FindById(id);
            var response = _mapper.Map<DataResponse<UserTypeDto>>(userType);
            return Ok(response);
        }
    }
}
