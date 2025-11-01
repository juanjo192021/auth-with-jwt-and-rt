using Auth.API.Common.Filters;
using Auth.API.Common.Requests;
using Auth.API.Common.Responses;
using Auth.API.Contracts.Responses;
using Auth.Application.Common.Dtos;
using Auth.Application.DTOs.UserType;
using Auth.Application.UseCases.UserTypeCases;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Auth.API.Contracts.Requests.UserType;
using Auth.Application.UseCases.UserTypeCase;

namespace Auth.API.Controllers
{
    [Route("user-types")]
    [ApiController]
    [Authorize]
    public class UserTypeController : Controller
    {
        private readonly FindAllUserTypesUseCase _findAllUseCase;
        private readonly FindUserTypeByIdUseCase _findByIdUseCase;
        private readonly CreateUserTypeUseCase _createUserTypeUseCase;
        private readonly UpdateUserTypeUseCase _updateUserTypeUseCase;
        private readonly DeactivateUserTypeUseCase _deactivateUserTypeUseCase;
        private readonly IMapper _mapper;

        public UserTypeController(FindAllUserTypesUseCase findAllUseCase,
            FindUserTypeByIdUseCase findByIdUseCase,
            CreateUserTypeUseCase createUserTypeUseCase,
            UpdateUserTypeUseCase updateUserTypeUseCase,
            DeactivateUserTypeUseCase deactivateUserTypeUseCase,
            IMapper mapper)
        {
            _findAllUseCase = findAllUseCase;
            _findByIdUseCase = findByIdUseCase;
            _createUserTypeUseCase = createUserTypeUseCase;
            _updateUserTypeUseCase = updateUserTypeUseCase;
            _deactivateUserTypeUseCase = deactivateUserTypeUseCase;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "ManageUsers")]
        public async Task<IActionResult> FindAll([FromQuery] PaginationRequest paginationRequest)
        {
            var paginationDto = _mapper.Map<PaginationDto>(paginationRequest);
            var userTypes = await _findAllUseCase.FindAllAsync(paginationDto);
            var response = _mapper.Map<PaginationResponse<List<UserTypeDto>>>(userTypes!);
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        [ValidateIdFilterAttribute]
        public async Task<IActionResult> FindOne(int id)
        {
            var userType = await _findByIdUseCase.FindByIdAsync(id);
            var response = _mapper.Map<DataResponse<UserTypeDto>>(userType!);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserTypeRequest createUserTypeRequest)
        {
            var createUserTypeDto = _mapper.Map<CreateUserTypeDto>(createUserTypeRequest);
            var result = await _createUserTypeUseCase.CreateAsync(createUserTypeDto);
            var response = _mapper.Map<DataResponse<UserTypeResponse>>(result);
            return Ok(response);
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateUserTypeRequest updateUserTypeRequest)
        {
            var updateUserTypeDto = _mapper.Map<UpdateUserTypeDto>(updateUserTypeRequest);
            var result = await _updateUserTypeUseCase.UpdateAsync(updateUserTypeDto.Id, updateUserTypeDto);
            var response = _mapper.Map<DataResponse<UserTypeResponse>>(result);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        [ValidateIdFilterAttribute]
        public async Task<IActionResult> Deactivate(int id)
        {
            var result = await _deactivateUserTypeUseCase.DeactiveAsync(id);
            var response = _mapper.Map<DataResponse<UserTypeResponse>>(result);
            return Ok(response);
        }
    }
}
