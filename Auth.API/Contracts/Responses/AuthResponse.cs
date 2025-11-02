using Auth.Application.DTOs.User;
using System.Text.Json.Serialization;

namespace Auth.API.Contracts.Responses
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public UserDto User { get; set; } = default!;
    }
}
