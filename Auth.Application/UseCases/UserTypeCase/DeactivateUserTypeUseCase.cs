using Auth.Application.Common.Exceptions;
using Auth.Application.Common.Responses;
using Auth.Application.DTOs.UserType;
using Auth.Domain.Entities;
using Auth.Domain.Interfaces;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.UseCases.UserTypeCase
{
    public class DeactivateUserTypeUseCase
    {
        private readonly IUserTypeRepository _userTypeRepository;
        private readonly IMapper _mapper;

        public DeactivateUserTypeUseCase(
            IUserTypeRepository userTypeRepository,
            IMapper mapper
            )
        {
            _userTypeRepository = userTypeRepository;
            _mapper = mapper;
        }

        public async Task<DataDto<UserTypeDto>> DeactiveAsync(int id)
        {
            var userTypeDeactivated = await _userTypeRepository.DeactiveAsync(id);

            if (userTypeDeactivated == null)
            {
                throw new NotFoundException($"Not found user type with ID {id}");
            }

            return new DataDto<UserTypeDto>
            {
                Data = _mapper.Map<UserTypeDto>(userTypeDeactivated)
            };
        }
    }
}
