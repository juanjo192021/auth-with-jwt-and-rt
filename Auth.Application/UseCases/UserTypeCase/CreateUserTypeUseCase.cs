using Auth.Application.Common.Responses;
using Auth.Application.DTOs.UserType;
using Auth.Domain.Interfaces;
using Auth.Domain.Entities;
using MapsterMapper;
using Auth.Application.Common.Exceptions;

namespace Auth.Application.UseCases.UserTypeCases
{
    public class CreateUserTypeUseCase
    {
        private readonly IUserTypeRepository _userTypeRepository;
        private readonly IMapper _mapper;

        public CreateUserTypeUseCase(
            IUserTypeRepository userTypeRepository,
            IMapper mapper
            )
        {
            _userTypeRepository = userTypeRepository;
            _mapper = mapper;
        }

        public async Task<DataDto<UserTypeDto>> CreateAsync(CreateUserTypeDto createUserTypeDto)
        {
            var userTypeEntity = _mapper.Map<UserType>(createUserTypeDto);
            var userType = await _userTypeRepository.CreateAsync(userTypeEntity);

            if (userType == null)
            {
                throw new InternalServerErrorException("Error al crear el tipo de usuario");
            }

            return new DataDto<UserTypeDto>
            {
                Data = _mapper.Map<UserTypeDto>(userType)
            };
        }
    }
}
