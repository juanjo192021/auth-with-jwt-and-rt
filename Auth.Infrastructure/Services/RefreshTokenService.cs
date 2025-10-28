using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Auth.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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

        public string GenerateRefreshToken()
        {
            //var byteArray = new byte[64];
            //var refreshToken = "";
            //using (var mg = RandomNumberGenerator.Create())
            //{
            //    mg.GetBytes(byteArray);
            //    refreshToken = Convert.ToBase64String(byteArray);
            //}

            //return refreshToken;

            var randomBytes = new byte[64];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }

        // User user, string refreshToken
        public async Task<string> SaveRefreshToken(int userId, string token, string refreshToken)
        {
            try
            {
                var now = DateTime.UtcNow;
                var refreshTokenExpiryDays = int.Parse(_configuration["Jwt:RefreshTokenExpiryDays"] ?? "7"); // por defecto 7 días

                var refreshTokenHistory = new RefreshTokenHistory
                {
                    UserId = userId,
                    Token = token,
                    RefreshToken = refreshToken,
                    CreationDate = now,
                    ExpirationDate = now.AddDays(refreshTokenExpiryDays),
                    IsActive = true
                };

                await _context.RefreshTokenHistories.AddAsync(refreshTokenHistory);
                await _context.SaveChangesAsync();

                return refreshToken;
            }
            catch (DbUpdateException ex)
            {
                // Puedes loguear el error
                throw new Exception("Error al guardar el refresh token en la base de datos.", ex);
            }
            catch (Exception ex)
            {
                // Catch genérico (solo si realmente quieres atrapar cualquier error inesperado)
                throw new Exception("Ocurrió un error inesperado al guardar el refresh token.", ex);
            }
        }

        public Task RevokeRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }


        public Task<bool> ValidateRefreshToken(User user, string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
