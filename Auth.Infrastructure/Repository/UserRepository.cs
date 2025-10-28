using Auth.Domain.Entities;
using Auth.Domain.Interfaces;
using Auth.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<User>?> FindAll ()
        {
            try
            {
                return await _context.Users.Include((u) => u.UserType).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User?> FindById(int id)
        {
            try
            {
                return await _context.Users.Include((u) => u.UserType).FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User?> FindOneByEmail(string email)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                throw ;
            }
        }

        public async Task<User?> Create(User user)
        {
            try
            {
                _context.Users.Add(user);
                int rows = await _context.SaveChangesAsync();

                return (user.Id > 0 && rows > 0) ? user : null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User?> Update (User user)
        {
            try
            {
                _context.Users.Update(user);
                int rows = await _context.SaveChangesAsync();
                return (rows > 0) ? user : null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<User?> BlockUser(int id)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null) return null;
                user.IsActive = true;
                _context.Users.Update(user);
                int rows = await _context.SaveChangesAsync();
                return (rows > 0) ? user : null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
