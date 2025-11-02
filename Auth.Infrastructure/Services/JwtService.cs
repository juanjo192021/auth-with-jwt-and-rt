using Auth.Application.Common.Exceptions;
using Auth.Application.Enums;
using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Genera un token JWT para el usuario
        public string GenerateToken(User user/*,IEnumerable<string> roles*/)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //var claims = new[]
            //{
            //    //ClaimTypes.NameIdentifier
            //    new Claim("id", user.Id.ToString()),
            //    new Claim(JwtRegisteredClaimNames.Iat,
            //      DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
            //      ClaimValueTypes.Integer64),
            //    //new Claim(ClaimTypes.Name, user.FirstName + ' '+ user.LastName )
            //};

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64),
            };

            if (user.UserRoles != null && user.UserRoles.Any())
            {
                foreach (var userRole in user.UserRoles)
                {
                    if (userRole.Role != null && userRole.Role.IsActive)
                    {
                        //ClaimTypes.Role
                        claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));
                    }
                }
            }

            var tokenExpiryMinutes = int.Parse(_configuration["Jwt:TokenExpiryMinutes"] ?? "60");


            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(tokenExpiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Verifica si el token JWT ha expirado
        public TokenStatus ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                // Token válido
                return TokenStatus.Valid;
            }
            catch (SecurityTokenExpiredException)
            {
                // Token expirado
                return TokenStatus.Expired;
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                return TokenStatus.InvalidSignature;
            }
            catch (ArgumentException)
            {
                return TokenStatus.InvalidFormat;
            }
            catch (Exception)
            {
                return TokenStatus.Corrupt;
            }
        }

        // Obtiene los claims del token JWT expirado
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

                if (securityToken is not JwtSecurityToken jwtToken ||
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    return null;

                return principal;
            }
            catch
            {
                return null;
            }
        }

    }
}
