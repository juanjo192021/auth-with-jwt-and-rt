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
    public class UserRoleRepository: IUserRoleRepository
    {
        private readonly AuthWithJwtAndRefreshTokenDbContext _context;

        public UserRoleRepository(AuthWithJwtAndRefreshTokenDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(int userId, List<int> roleIds)
        {
            // Validar existencia del usuario
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);

            if (!userExists)
                throw new Exception($"El usuario con id {userId} no existe.");

            // Obtener roles válidos
            var validRoles = await _context.Roles
                .Where(r => roleIds.Contains(r.Id) && r.IsActive) // Asegurarse de que el rol esté activo
                .Select(r => r.Id) // Solo retornar los IDs de los roles
                .ToListAsync();

            // Verificar si se encontraron roles válidos
            if (!validRoles.Any())
                throw new Exception("No se encontraron roles válidos para asignar.");

            // Crear lista de relaciones
            var userRoles = validRoles.Select(roleId => new UserRole
            {
                UserId = userId,
                RoleId = roleId,
            }).ToList();

            // Guardar en BD
            await _context.UserRoles.AddRangeAsync(userRoles);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }
    }
}
