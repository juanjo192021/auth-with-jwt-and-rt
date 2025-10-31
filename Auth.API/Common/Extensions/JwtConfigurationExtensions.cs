using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Auth.API.Common.Extensions
{
    public static class JwtConfigurationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    RoleClaimType = ClaimTypes.Role,  // define el claim que se usará como rol
                    ClockSkew = TimeSpan.Zero
                };

                // Captura de eventos de autenticación/autorización
                options.Events = new JwtBearerEvents
                {
                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            message = "No tienes permisos suficientes para acceder a este recurso."
                        }));
                    },
                    OnChallenge = async context =>
                    {
                        context.HandleResponse(); // evita la respuesta por defecto
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            message = "Token inválido o faltante."
                        }));
                    }
                };
            });

            // Authorization → políticas (opcional)
            //builder.Services.AddAuthorization(options =>
            //{
            //    // política simple que requiere cualquiera de esos roles
            //    options.AddPolicy("ManageUsersPolicy", policy =>
            //        policy.RequireRole("ManageUsers", "Admin"));

            //    // política por permiso más específico:
            //    options.AddPolicy("AccessDashboardPolicy", policy =>
            //        policy.RequireRole("AccessDashboard", "Admin"));
            //});

            return services;
        }
    }
}
