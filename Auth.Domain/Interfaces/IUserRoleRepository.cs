using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Domain.Interfaces
{
    public interface IUserRoleRepository
    {
        public Task<bool> CreateAsync(int userId, List<int> roleIds);
    }
}
