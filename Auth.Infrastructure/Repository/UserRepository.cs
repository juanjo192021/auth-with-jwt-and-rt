using Auth.Domain.Entities;
using Auth.Domain.Interfaces;
using Auth.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly AuthWithJwtAndRefreshTokenDbContext _context;

        public UserRepository(AuthWithJwtAndRefreshTokenDbContext context)
        {
            _context = context;
        }

        public IQueryable<User> GetAllQueryable()
        {
            return _context.Users.AsQueryable();
        }

        public async Task<List<User>?> FindAllAsync()
        {
            return await _context.Users.Include((u) => u.UserType).ToListAsync();
        }

        public async Task<User?> FindByIdAsync(int id)
        {
            return await _context.Users.Include((u) => u.UserType).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            return await _context.Users.Include(u => u.UserType)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> CreateAsync(User user)
        {
            _context.Users.Add(user);
            int rows = await _context.SaveChangesAsync();

            return (user.Id > 0 && rows > 0) ? user : null;
        }

        public async Task<User?> UpdateAsync (User user)
        {
            _context.Users.Update(user);
            int rows = await _context.SaveChangesAsync();
            return (rows > 0) ? user : null;
        }

        public async Task<User?> BlockUserAsync(int id)
        {
            //var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            var user = await FindByIdAsync(id);
            if (user == null) return null;
            user.IsActive = false;
            _context.Users.Update(user);
            int rows = await _context.SaveChangesAsync();
            return (rows > 0) ? user : null;
        }
    }
}
