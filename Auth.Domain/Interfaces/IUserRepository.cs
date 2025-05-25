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
        public Task<User?> GetByUsernameAsync(string username);
    }
}
