using Auth.Domain.Entities;
using Auth.Domain.Interfaces;
using Auth.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repository
{
    public class UserRoleRepository: IUserRoleRepository
    {
        private readonly AuthWithJwtAndRefreshTokenDbContext _context;

        public UserRoleRepository(AuthWithJwtAndRefreshTokenDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(int userId, List<int> roleIds)
        {
            // Obtener roles válidos
            var validRoles = await _context.Roles
                .Where(r => roleIds.Contains(r.Id) && r.IsActive) // Asegurarse de que el rol esté activo
                .Select(r => r.Id) // Solo retornar los IDs de los roles
                .ToListAsync();

            // Verificar si se encontraron roles válidos
            if (!validRoles.Any())
                return 0;

            // Crear lista de relaciones
            var userRoles = validRoles.Select(roleId => new UserRole
            {
                UserId = userId,
                RoleId = roleId,
                IsActive = true
            }).ToList();

            // Guardar en BD
            await _context.UserRoles.AddRangeAsync(userRoles);
            var result = await _context.SaveChangesAsync();

            return result;
        }
    }
}
