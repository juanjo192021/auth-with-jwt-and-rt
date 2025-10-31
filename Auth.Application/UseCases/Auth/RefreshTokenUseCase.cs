using Auth.Application.Common.Exceptions;
using Auth.Application.DTOs.Auth;
using Auth.Application.Enums;
using Auth.Application.Interfaces;
using Auth.Domain.Interfaces;
using MapsterMapper;
using System.Security.Claims;

namespace Auth.Application.UseCases.Auth
{
    public class RefreshTokenUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IMapper _mapper;

        public RefreshTokenUseCase(
            IUserRepository userRepository,
            IJwtService jwtService,
            IRefreshTokenService refreshTokenService,
            IMapper mapper
            )
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
            _mapper = mapper;
        }

        public async Task<AuthResultDto> RefreshToken(RefreshTokenDto refreshTokenDto)
        {
            // Obtener la entidad del refresh token
            var tokenEntity = await _refreshTokenService.FindByRefreshTokenAsync(refreshTokenDto.RefreshToken);

            // Verificar que el refresh token exista y esté activo
            if (tokenEntity == null)
                throw new UnauthorizedException("Refresh token no existe.");
            
            // Verificar que el refresh token esté activo
            if (!tokenEntity.IsActive)
                throw new UnauthorizedException("Refresh token fue revocado.");

            // Verificar que el JWT enviado coincida con el último token asociado al refresh token
            if (tokenEntity.Token != refreshTokenDto.Token)
                throw new BadRequestException("El token enviado ya no corresponde al refresh token activo.");
            
            // Verificar si el refresh token ha expirado
            if (tokenEntity.ExpirationDate <= DateTime.UtcNow)
            {
                tokenEntity.IsActive = false;
                await _refreshTokenService.UpdateAsync(tokenEntity);
                throw new UnauthorizedException("Refresh token ha expirado.");
            }

            // Verificar si el token todavía es válido
            var tokenStatus = _jwtService.ValidateToken(refreshTokenDto.Token);

            switch (tokenStatus)
            {
                case TokenStatus.Valid:
                    throw new BadRequestException("El token aún es válido, no es necesario renovarlo.");
                case TokenStatus.InvalidFormat:
                    throw new BadRequestException("El token enviado no tiene un formato válido.");
                case TokenStatus.InvalidSignature:
                    throw new BadRequestException("El token enviado fue manipulado o la firma es inválida.");
                case TokenStatus.Corrupt:
                    throw new BadRequestException("Token inválido o corrupto.");
                case TokenStatus.Expired:
                    break; // seguimos con la renovación
            }

            // Obtener los claims del token para la renovación
            var principal = _jwtService.GetPrincipalFromExpiredToken(refreshTokenDto.Token);

            if (principal == null)
                throw new BadRequestException("Token inválido o corrupto, no se pudo obtener información");

            // Obtener el claim del ID de usuario
            //var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "id");
            //var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Verificar que el usuario exista
            if (userIdClaim == null)
                throw new BadRequestException("No se encontró atributos en el token.");

            var userId = int.Parse(userIdClaim);

            var user = await _userRepository.FindByIdAsync(userId);
            if (user == null)
                throw new BadRequestException("Usuario no encontrado.");
            
            // Invalidar el refresh token anterior
            tokenEntity.IsActive = false;
            await _refreshTokenService.UpdateAsync(tokenEntity);

            // Generar nuevos tokens
            var newToken = _jwtService.GenerateToken(user);
            var newRefreshToken = _refreshTokenService.GenerateRefreshToken();

            await _refreshTokenService.CreateAsync(user.Id, newToken, newRefreshToken);

            return new AuthResultDto
            {
                Token = newToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}
