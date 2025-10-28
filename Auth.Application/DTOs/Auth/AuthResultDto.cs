using Auth.Application.DTOs.User;

namespace Auth.Application.DTOs.Auth
{
    public class AuthResultDto
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public UserDto User { get; set; } = null!;
    }
}
