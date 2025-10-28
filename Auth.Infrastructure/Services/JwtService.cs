using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Services
{
    public class JwtService : IJwtService
    {

        //"Jwt": {
        //  "Key": "clave-secreta-bien-larga",
        //  "Issuer": "TuApp",
        //  "Audience": "TuAppUsers"
        //}

        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Genera un token JWT para el usuario dado
        public string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                //new Claim(ClaimTypes.Name, user.FirstName + ' '+ user.LastName )
            };

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

        // Extrae los claims del token si ha expirado, o devuelve null si aún es válido
        public User? GetClaimsIfTokenExpired(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Leer el token (sin validarlo aún)
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Validar si aún no ha expirado
            if (jwtToken.ValidTo > DateTime.UtcNow)
            {
                return null; // Token aún válido
            }

            // Token expirado, extraemos claims manualmente
            var idClaim = jwtToken.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier || c.Type == JwtRegisteredClaimNames.NameId);

            // 
            //var usernameClaim = jwtToken.Claims.FirstOrDefault(c =>
            //    c.Type == ClaimTypes.Name || c.Type == JwtRegisteredClaimNames.UniqueName);

            if (idClaim == null /*|| usernameClaim == null*/)
                return null;

            return new User
            {
                Id = int.Parse(idClaim.Value),
                /*Email = usernameClaim.Value*/
            };
        }
    }
}
