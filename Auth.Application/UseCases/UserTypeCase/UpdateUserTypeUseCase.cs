using Auth.Application.Common.Exceptions;
using Auth.Application.Common.Responses;
using Auth.Application.DTOs.UserType;
using Auth.Domain.Entities;
using Auth.Domain.Interfaces;
using Mapster;
using MapsterMapper;

namespace Auth.Application.UseCases.UserTypeCases
{
    public class UpdateUserTypeUseCase
    {
        private readonly IUserTypeRepository _userTypeRepository;
        private readonly IMapper _mapper;

        public UpdateUserTypeUseCase(
            IUserTypeRepository userTypeRepository,
            IMapper mapper
            )
        {
            _userTypeRepository = userTypeRepository;
            _mapper = mapper;
        }

        public async Task<DataDto<UserTypeDto>> UpdateAsync(int id, UpdateUserTypeDto updateUserTypeDto)
        {
            var userType = await _userTypeRepository.FindByIdAsync(id);

            if (userType == null)
                throw new NotFoundException($"Not found user type with ID {id}");

            // 🔹 Aplica solo los cambios no nulos sobre la entidad existente
            updateUserTypeDto.Adapt(userType);

            var userTypeUpdated = await _userTypeRepository.UpdateAsync(userType);

            if (userTypeUpdated == null)
            {
                throw new InternalServerErrorException("Error updating user type");
            }

            return new DataDto<UserTypeDto>
            {
                Data = _mapper.Map<UserTypeDto>(userType)
            };
        }
    }
}
