using Auth.Application.Common.Exceptions;
using Auth.Application.DTOs.Auth;
using Auth.Application.DTOs.User;
using Auth.Application.Interfaces;
using Auth.Domain.Interfaces;
using MapsterMapper;

namespace Auth.Application.UseCases.Auth
{
    public class LoginUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IMapper _mapper;

        public LoginUseCase(
            IUserRepository userRepository,
            IJwtService jwtService,
            IPasswordHasher passwordHasher,
            IRefreshTokenService refreshTokenService,
            IMapper mapper
            )
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _refreshTokenService = refreshTokenService;
            _mapper = mapper;
        }

        public async Task<AuthResultDto> Login(LoginDto loginRequest)
        {
            var email = loginRequest.Email;
            var password = loginRequest.Password;

            var user = await _userRepository.FindByEmailAsync(email);
            if (user == null)
                throw new NotFoundException($"No se encontró un usuario con el email {email}");

            user.Password = _passwordHasher.Hash(password);

            bool validPassword = _passwordHasher.Verify(password, user.Password);
            if (!validPassword)
                throw new BadRequestException("Email / Password inválidos");

            // Generar el token JWT
            var token = _jwtService.GenerateToken(user);

            // Generar el refresh token
            var refreshToken = _refreshTokenService.GenerateRefreshToken();

            // Guardar el refresh token en la base de datos
            await _refreshTokenService.CreateAsync(user.Id, token, refreshToken);

            return new AuthResultDto
            {
                Token = token,
                RefreshToken = refreshToken,
                User = _mapper.Map<UserDto>(user)
            };
        }

    }
}
