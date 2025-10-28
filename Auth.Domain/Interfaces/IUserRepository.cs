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

        Task<List<User>?> FindAll();

        Task<User?> FindById(int id);

        Task<User?> FindOneByEmail(string email);

        Task<User?> Create(User user);

        Task<User?> Update(User user);

        Task<User?> BlockUser(int id);
    }
}
