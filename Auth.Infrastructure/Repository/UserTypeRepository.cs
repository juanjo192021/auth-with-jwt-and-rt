using Auth.Domain.Entities;
using Auth.Domain.Interfaces;
using Auth.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repository
{
    public class UserTypeRepository: IUserTypeRepository
    {
        private readonly AuthWithJwtAndRefreshTokenDbContext _context;

        public UserTypeRepository(AuthWithJwtAndRefreshTokenDbContext context) {
            _context = context;
        }

        public async Task<UserType?> CreateAsync(UserType userType)
        {
            _context.UserTypes.Add(userType);
            int rows = await _context.SaveChangesAsync();

            return (userType.Id > 0 && rows > 0) ? userType : null;
        }

        public async Task<UserType?> DeactiveAsync(int id)
        {
            var userType = await FindByIdAsync(id);
            if (userType == null) return null;
            userType.IsActive = false;
            _context.UserTypes.Update(userType);
            int rows = await _context.SaveChangesAsync();
            return (rows > 0) ? userType : null;
        }

        public async Task<(List<UserType> Data, int TotalRecords)> FindAllAsync(int pageSize, int page, string? search)
        {
            IQueryable<UserType> query = _context.UserTypes;

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m => EF.Functions.Like(m.Name.ToLower(), $"%{search.ToLower()}%"))
                             .OrderBy(m => m.Id);
            }

            int totalRecords = await query.CountAsync();

            var data = await query.OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToListAsync();
            
            return (data, totalRecords);
        }

        public async Task<UserType?> FindByIdAsync(int id)
        {
            return await _context.UserTypes.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UserType?> UpdateAsync(UserType userType)
        {
            _context.UserTypes.Update(userType);
            int rows = await _context.SaveChangesAsync();
            return (rows > 0) ? userType : null;
        }
    }
}
