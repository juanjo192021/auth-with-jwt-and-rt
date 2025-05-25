using Auth.Application.Interfaces;
using Auth.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.UseCases.Auth
{
    public class LoginHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher _passwordHasher;

        public LoginHandler(IUserRepository userRepository, IJwtService jwtService, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> HandleAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
                throw new Exception("Usuario no encontrado");

            bool validPassword = _passwordHasher.Verify(password, user.Password);
            if (!validPassword)
                throw new Exception("Contraseña inválida");

            return _jwtService.GenerateToken(user);
        }
    }
}
