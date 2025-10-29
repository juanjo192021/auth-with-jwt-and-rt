using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Auth.Application.DTOs.Auth;
using Auth.Application.UseCases.Auth;
using Auth.API.Contracts.Requests;
using MapsterMapper;

namespace Auth.API.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly LoginUseCase _loginUseCase;
        private readonly SignupUseCase _signupUseCase;
        private readonly RefreshTokenUseCase _refreshTokenUseCase;
        private readonly IMapper _mapper;

        public AuthController(LoginUseCase loginUseCase,
            SignupUseCase signupUseCase,
            RefreshTokenUseCase refreshTokenUseCase,
            IMapper mapper)
        {
            _loginUseCase = loginUseCase;
            _signupUseCase = signupUseCase;
            _refreshTokenUseCase = refreshTokenUseCase;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var loginDto = _mapper.Map<LoginDto>(loginRequest);

            var authResponse = await _loginUseCase.Login(loginDto);

            return Ok(authResponse);
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> Signup([FromBody] SignupRequest signupRequest)
        {
            var signupDto = _mapper.Map<SignupDto>(signupRequest);

            var authResponse = await _signupUseCase.Signup(signupDto);
            
            return Ok(authResponse);
        }

        [HttpPost]
        [Route("refreshToken")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            var refreshTokenDto = _mapper.Map<RefreshTokenDto>(refreshTokenRequest);

            var authResponse = await _refreshTokenUseCase.RefreshToken(refreshTokenDto);

            return Ok(authResponse);
        }
    }
}
