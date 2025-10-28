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
        public string GenerateRefreshToken();
        public Task<string> SaveRefreshToken(int userId, string token, string refreshToken);
        public Task<bool> ValidateRefreshToken(User user, string refreshToken);
        public Task RevokeRefreshToken(string refreshToken);
    }
}
