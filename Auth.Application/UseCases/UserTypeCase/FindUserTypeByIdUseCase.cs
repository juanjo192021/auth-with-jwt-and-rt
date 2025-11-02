using Auth.Application.Common.Exceptions;
using Auth.Application.Common.Responses;
using Auth.Application.DTOs.UserType;
using Auth.Domain.Interfaces;
using MapsterMapper;

namespace Auth.Application.UseCases.UserTypeCases
{
    public class FindUserTypeByIdUseCase
    {
        private readonly IUserTypeRepository _userTypeRepository;
        private readonly IMapper _mapper;

        public FindUserTypeByIdUseCase(
            IUserTypeRepository userTypeRepository,
            IMapper mapper
            )
        {
            _userTypeRepository = userTypeRepository;
            _mapper = mapper;
        }

        public async Task<DataDto<UserTypeDto>?> FindByIdAsync(int id)
        {
            var userType = await _userTypeRepository.FindByIdAsync(id);
            if (userType == null)
            {
                throw new NotFoundException($"No se encontró un usuario con el ID {id}");
            }
            return new DataDto<UserTypeDto>
            {
                Data = _mapper.Map<UserTypeDto>(userType)
            };
        }
    }
}
