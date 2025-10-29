using Auth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain.Interfaces
{
    public interface IUserRepository
    {
        public IQueryable<User> GetAllQueryable();

        Task<List<User>?> FindAllAsync();

        Task<User?> FindByIdAsync(int id);

        Task<User?> FindByEmailAsync(string email);

        Task<User?> CreateAsync(User user);

        Task<User?> UpdateAsync(User user);

        Task<User?> BlockUserAsync(int id);
    }
}
