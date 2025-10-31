using Auth.API.Common.Dtos;
using Auth.Application.Common.Dtos;
using Auth.Application.Common.Responses;
using Auth.Application.DTOs.Auth;
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
        private readonly IMapper _mapper;

        public UserTypeController(FindAllUseCase findAllUseCase, IMapper mapper)
        {
            _findAllUseCase = findAllUseCase;
            _mapper = mapper;
        }
        
        [HttpGet]
        [Authorize(Roles = "ManageUsers")]
        public async Task<IActionResult> FindAll([FromQuery] PaginationRequest paginationRequest)
        {
            var paginationDto = _mapper.Map<PaginationDto>(paginationRequest);
            var userTypes = await _findAllUseCase.FindAll(paginationDto);
            var response = _mapper.Map<PaginationResponse<List<UserTypeDto>>>(userTypes);
            return Ok(userTypes);
        }
    }
}
