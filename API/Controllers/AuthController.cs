using Application.DTOs.Auth;
using Application.DTOs.Instructor;
using Application.DTOs.User;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("easytrainer/api/v1/auth")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginInputDTO dto)
        {
            var result = await _authService.AuthenticateAsync(dto);
            return Ok(result);
        }

        [HttpPost("register/user/basic")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUserBasic([FromBody] CreateUserRegisterDTO dto)
        {
            var result = await _authService.RegisterUserBasicAsync(dto);
            return Ok(result);
        }

        [HttpPost("register/instructor/basic")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterInstructorBasic([FromBody] CreateInstructorRegisterDTO dto)
        {
            var result = await _authService.RegisterInstructorBasicAsync(dto);
            return Ok(result);
        }

        [HttpGet("validate")]
        [Authorize]
        public IActionResult ValidateToken()
        {
            return Ok(new { valid = true });
        }

    }

}
