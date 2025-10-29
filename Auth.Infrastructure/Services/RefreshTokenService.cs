using Auth.Application.Enums;
using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Auth.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using Auth.Application.Common.Exceptions;

namespace Auth.Infrastructure.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly AuthWithJwtAndRefreshTokenDbContext _context;
        private readonly IConfiguration _configuration;

        public RefreshTokenService(AuthWithJwtAndRefreshTokenDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Genera un nuevo refresh token
        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }

        // Guarda una nueva entidad RefreshTokenHistory en la base de datos
        public async Task<string> CreateAsync(int userId, string token, string refreshToken)
        {
            var now = DateTime.UtcNow;
            var refreshTokenExpiryDays = int.Parse(_configuration["Jwt:RefreshTokenExpiryDays"] ?? "7"); // por defecto 7 días

            var refreshTokenHistory = new RefreshTokenHistory
            {
                UserId = userId,
                Token = token,
                RefreshToken = refreshToken,
                CreationDate = now,
                // AQUI OJO
                ExpirationDate = now.AddDays(refreshTokenExpiryDays),
                IsActive = true
            };

            await _context.RefreshTokenHistories.AddAsync(refreshTokenHistory);
            await _context.SaveChangesAsync();

            return refreshToken;
        }

        // Busca la entidad RefreshTokenHistory en la base de datos basado en el valor del refresh token
        public async Task<RefreshTokenHistory?> FindByRefreshTokenAsync(string refreshToken)
        {
            return await _context.RefreshTokenHistories
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
        }

        // Actualiza la entidad RefreshTokenHistory en la base de datos
        public async Task UpdateAsync(RefreshTokenHistory tokenEntity)
        {
            _context.RefreshTokenHistories.Update(tokenEntity);
            await _context.SaveChangesAsync();
        }
    }
}
