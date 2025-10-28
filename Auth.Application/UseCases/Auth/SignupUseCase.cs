using Auth.Application.DTOs.Auth;
using Auth.Application.DTOs.User;
using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Auth.Domain.Interfaces;
using MapsterMapper;

namespace Auth.Application.UseCases.Auth
{
    public class SignupUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IMapper _mapper;

        public SignupUseCase(
            IUserRepository userRepository, 
            IJwtService jwtService, 
            IPasswordHasher passwordHasher, 
            IRefreshTokenService refreshTokenService,
            IUserRoleRepository userRoleRepository,
            IMapper mapper
            )
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _userRoleRepository = userRoleRepository;
            _refreshTokenService = refreshTokenService;
            _mapper = mapper;
        }

        public async Task<AuthResultDto> Signup(SignupDto signupRequest)
        {
            // Encriptar la contraseña
            var hashedPassword = _passwordHasher.Hash(signupRequest.Password);

            // Transformar SignupRequest a User
            var userEntity = _mapper.Map<User>(signupRequest);

            // Asignar la contraseña encriptada
            userEntity.Password = hashedPassword;

            // Guardar el usuario en la base de datos
            var user = await _userRepository.Create(userEntity);

            // Validar que el usuario se haya creado correctamente
            if (user == null)
                throw new InvalidOperationException("Error al crear el usuario");

            // Asignar roles al usuario
            var response = await _userRoleRepository.CreateAsync(user.Id, signupRequest.Roles);

            if (!response)
                throw new InvalidOperationException("Error al asignar roles al usuario");

            // Generar el token JWT
            var token = _jwtService.GenerateToken(user);

            // Generar el refresh token
            var refreshToken = _refreshTokenService.GenerateRefreshToken();

            // Guardar el refresh token en la base de datos
            await _refreshTokenService.SaveRefreshToken(user.Id, token, refreshToken);

            return new AuthResultDto
            {
                Token = token,
                RefreshToken = refreshToken,
                User = _mapper.Map<UserDto>(user)
            };
        }
    }
}
