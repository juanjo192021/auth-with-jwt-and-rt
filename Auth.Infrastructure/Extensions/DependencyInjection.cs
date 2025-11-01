using Auth.Application.Interfaces;
using Auth.Application.Mappings;
using Auth.Application.UseCases.Auth;
using Auth.Application.UseCases.UserTypeCase;
using Auth.Application.UseCases.UserTypeCases;
using Auth.Domain.Interfaces;
using Auth.Infrastructure.Persistence;
using Auth.Infrastructure.Repository;
using Auth.Infrastructure.Services;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<AuthWithJwtAndRefreshTokenDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("cadenaSQL")));

            // Mapster
            // Registrar configuraciones personalizadas
            MapsterConfiguration.RegisterMappings();

            // Registrar Mapster con DI
            var config = TypeAdapterConfig.GlobalSettings;
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            // Repositorios
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IUserTypeRepository, UserTypeRepository>();

            // Servicios concretos
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            // (Opcional) Registra usecases si quieres centralizar aquí
            services.AddScoped<LoginUseCase>();
            services.AddScoped<SignupUseCase>();
            services.AddScoped<RefreshTokenUseCase>();
            services.AddScoped<FindAllUserTypesUseCase>();
            services.AddScoped<FindUserTypeByIdUseCase>();
            services.AddScoped<CreateUserTypeUseCase>();
            services.AddScoped<UpdateUserTypeUseCase>();
            services.AddScoped<DeactivateUserTypeUseCase>();

            return services;
        }
    }
}
