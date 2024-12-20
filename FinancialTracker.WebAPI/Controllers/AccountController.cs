using FinancialTracker.Application.DTOs;
using FinancialTracker.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FinancialTracker.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtTokenService _jwtTokenService;
        private readonly UserService _userService;

        public AccountController(JwtTokenService jwtTokenService, UserService userService)
        {
            _jwtTokenService = jwtTokenService;
            _userService = userService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userService.ValidateUserAsync(loginDto.Username, loginDto.Password);
            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid username or password." });
            }

            // Generate JWT token with roles
            var token = _jwtTokenService.GenerateToken(user.UserId, user.Username, user.Roles.Select(r=>r.RoleName).ToList());

            return Ok(new
            {
                Token = token,
                Message = "Login successful."
            });
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _userService.RegisterUserAsync(registerDto);
            if (!result.Success)
            {
                return BadRequest(new { Message = result.Message });
            }

            return Ok(new { Message = "Registration successful." });
        }
    }
}
