using FinancialTracker.Application.Services;
using FinancialTracker.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FinancialTracker.Application.DTOs;
using System.Security.Claims;

namespace FinancialTracker.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        // Helper method to extract UserId from JWT
        private int GetUserId()
        {
            if (!User.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException("User is not authenticated.");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("User ID is missing in the token.");

            return int.Parse(userIdClaim);
        }

        [HttpGet("GetUserByID/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new { Message = $"User with ID {id} not found." });

            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // Update logged-in user's profile
        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserDto updateUserDto)
        {
            try
            {
                int userId = GetUserId(); // Retrieve UserId from JWT

                // Ensure the logged-in user is updating their own profile
                if (userId != updateUserDto.Id)
                    return Forbid("You are not allowed to update another user's profile.");

                var result = await _userService.UpdateUserAsync(updateUserDto);
                if (!result.Success)
                    return BadRequest(new { Message = result.Message });

                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result.Success)
                return BadRequest(new { Message = result.Message });

            return NoContent();
        }
    }
}
