using Auth.Application.Enums;
using Auth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Interfaces
{
    public interface IRefreshTokenService
    {
        string GenerateRefreshToken();
        Task<string> CreateAsync(int userId, string token, string refreshToken);
        Task<RefreshTokenHistory?> FindByRefreshTokenAsync(string refreshToken);
        Task UpdateAsync(RefreshTokenHistory tokenEntity);
    }
}
