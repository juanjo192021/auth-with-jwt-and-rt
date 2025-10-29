using Auth.Application.Enums;
using Auth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        //bool IsValidTokenStructure(string token);
        TokenStatus ValidateToken(string token);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
