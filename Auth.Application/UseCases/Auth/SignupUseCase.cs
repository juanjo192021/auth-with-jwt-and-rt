using Auth.Application.Common.Exceptions;
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
         
            var hashedPassword = _passwordHasher.Hash(signupRequest.Password);
            var userEntity = _mapper.Map<User>(signupRequest);
            userEntity.Password = hashedPassword;
            userEntity.IsActive = true;
            userEntity.IsConfirmed = true;

            var user = await _userRepository.CreateAsync(userEntity);

            if (user == null)
                throw new InternalServerErrorException("Error al guardar usuario en la BD");

            var added = await _userRoleRepository.CreateAsync(user.Id, signupRequest.Roles);

            if (added == 0)
                throw new BadRequestException("No se encontraron roles válidos para asignar.");

            var token = _jwtService.GenerateToken(user);

            var refreshToken = _refreshTokenService.GenerateRefreshToken();

            await _refreshTokenService.CreateAsync(user.Id, token, refreshToken);

            user = await _userRepository.FindByIdAsync(user.Id);

            return new AuthResultDto
            {
                Token = token,
                RefreshToken = refreshToken,
                User = _mapper.Map<UserDto>(user!)
            };
        }
    }
}
