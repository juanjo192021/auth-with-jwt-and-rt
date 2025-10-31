using Auth.Domain.Entities;

namespace Auth.Domain.Interfaces
{
    public interface IUserTypeRepository
    {
        Task<(List<UserType> Data, int TotalRecords)> FindAllAsync(int pageSize, int page, string? search);
        Task<UserType?> FindByIdAsync(int id);
        Task<UserType?> CreateAsync(UserType userType);
        Task<UserType?> UpdateAsync(UserType userType);
        Task<UserType?> DeactiveAsync(int id);
    }
}
