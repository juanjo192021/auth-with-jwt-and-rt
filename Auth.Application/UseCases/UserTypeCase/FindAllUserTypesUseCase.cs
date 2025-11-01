using Auth.Application.Common.Dtos;
using Auth.Application.Common.Responses;
using Auth.Application.DTOs.UserType;
using Auth.Domain.Interfaces;
using MapsterMapper;

namespace Auth.Application.UseCases.UserTypeCases
{
    public class FindAllUserTypesUseCase
    {
        private readonly IUserTypeRepository _userTypeRepository;
        private readonly IMapper _mapper;

        public FindAllUserTypesUseCase(
            IUserTypeRepository userTypeRepository,
            IMapper mapper
            )
        {
            _userTypeRepository = userTypeRepository;
            _mapper = mapper;
        }

        public async Task<PaginationResultDto<List<UserTypeDto>>?> FindAllAsync(PaginationDto paginationDto)
        {
            var page = paginationDto.Page;
            var pageSize = paginationDto.PageSize;
            var search = paginationDto.Search;

            var (userTypes, totalRecords) = await _userTypeRepository.FindAllAsync(pageSize, page, search);

            if (userTypes == null)
            {
                return null;
            }

            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            return new PaginationResultDto<List<UserTypeDto>>
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                HasPreviousPage = page > 1,
                HasNextPage = page < totalPages,
                Data = _mapper.Map<List<UserTypeDto>>(userTypes)
            };
        }
    }
}
