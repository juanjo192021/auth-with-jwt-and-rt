using Auth.API.Common.Constants;
using Auth.API.Common.Responses;
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

                        var error = new ErrorResponse
                        {
                            Type = ErrorTypeUris.Forbidden,
                            Title = "Acceso denegado",
                            Status = StatusCodes.Status403Forbidden,
                            Errors = "No tienes permisos suficientes para acceder a este recurso.",
                            TraceId = context.HttpContext.TraceIdentifier,
                        };

                        await context.Response.WriteAsync(JsonSerializer.Serialize(error));
                    },
                    OnChallenge = async context =>
                    {
                        context.HandleResponse(); // evita la respuesta por defecto
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        var error = new ErrorResponse
                        {
                            Type = ErrorTypeUris.Unauthorized,
                            Title = "No autorizado",
                            Status = StatusCodes.Status401Unauthorized,
                            Errors = "Token inválido, expirado o faltante. Incluye un token JWT válido en el encabezado Authorization: Bearer <token>.",
                            TraceId = context.HttpContext.TraceIdentifier,
                        };

                        await context.Response.WriteAsync(JsonSerializer.Serialize(error));
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
