using Auth.Application.Common.Exceptions;
using Auth.Application.DTOs.Auth;
using Auth.Application.DTOs.User;
using Auth.Application.Interfaces;
using Auth.Application.UseCases.Auth;
using Auth.Domain.Entities;
using Auth.Domain.Interfaces;
using FluentAssertions;
using MapsterMapper;
using Moq;

namespace Auth.UnitTests.Application.Auth
{
    public class LoginUseCaseTests
    {
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly Mock<IJwtService> _jwtService = new();
        private readonly Mock<IPasswordHasher> _passwordHasher = new();
        private readonly Mock<IRefreshTokenService> _refreshTokenService = new();
        private readonly Mock<IMapper> _mapper = new();

        private LoginUseCase CreateUseCase() => new(
            _userRepository.Object,
            _jwtService.Object,
            _passwordHasher.Object,
            _refreshTokenService.Object,
            _mapper.Object
        );

        // User not found
        [Fact]
        public async Task Should_Throw_NotFoundException_When_User_Not_Found()
        {
            // Arrange: Moq intercepta la llamada a FindByEmailAsync y devuelve null
            // simula que el usuario no existe en la base de datos
            _userRepository.Setup(r => r.FindByEmailAsync("cualquiercorreo@google.com"))
                     .ReturnsAsync((User?)null);

            var useCase = CreateUseCase();

            // Act: Intentamos hacer login con un email que no existe
            var act = async () => await useCase.Login(new LoginDto { Email = "test@google.com", Password = "123456" });

            // Assert: Verificamos que se lance una NotFoundException
            await act.Should().ThrowAsync<NotFoundException>()
                     .WithMessage("*test@google.com*");
        }


        // Password invalid
        [Fact]
        public async Task Should_Throw_BadRequestException_When_Password_Invalid()
        {
            // Arrange
            var user = new User { Email = "juan@google.com", Password = "hashed" };
            _userRepository.Setup(r => r.FindByEmailAsync(user.Email)).ReturnsAsync(user);
            _passwordHasher.Setup(h => h.Hash("12345")).Returns("hashed");
            _passwordHasher.Setup(h => h.Verify("1234", "hashed")).Returns(false);

            var useCase = CreateUseCase();

            // Act
            var act = async () => await useCase.Login(new LoginDto { Email = user.Email, Password = "1234" });

            // Assert
            await act.Should().ThrowAsync<BadRequestException>()
                     .WithMessage("Email / Password inválidos");
        }

        // Successful login
        [Fact]
        public async Task Should_Return_Tokens_When_Login_Is_Valid()
        {
            // Arrange
            var user = new User { Id = 1, Email = "juan@google.com", Password = "hashed" };

            _userRepository.Setup(r => r.FindByEmailAsync(user.Email)).ReturnsAsync(user);
            _passwordHasher.Setup(h => h.Hash("1234")).Returns("hashed");
            _passwordHasher.Setup(h => h.Verify("1234", "hashed")).Returns(true);
            _jwtService.Setup(j => j.GenerateToken(user)).Returns("jwt-token");
            _refreshTokenService.Setup(r => r.GenerateRefreshToken()).Returns("refresh-token");
            _mapper.Setup(m => m.Map<UserDto>(user)).Returns(new UserDto { Email = user.Email });

            var useCase = CreateUseCase();

            // Act
            var result = await useCase.Login(new LoginDto { Email = user.Email, Password = "1234" });

            // Assert
            result.Token.Should().Be("jwt-token");
            result.RefreshToken.Should().Be("refresh-token");
            result.User.Email.Should().Be(user.Email);

            _refreshTokenService.Verify(r => r.CreateAsync(user.Id, "jwt-token", "refresh-token"), Times.Once);
        }
    }
}
