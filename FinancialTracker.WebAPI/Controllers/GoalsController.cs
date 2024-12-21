using FinancialTracker.Application.Services;
using FinancialTracker.Domain.Entites;
using FinancialTracker.Domain.Entities;
using FinancialTracker.Infrastructure.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GoalsController : BaseController
    {
        private readonly GoalsService _goalsService;

        public GoalsController(GoalsService goalsService)
        {
            _goalsService = goalsService;
        }

        // Get all goals for the logged-in user
        [HttpGet("Get_All_Goals")]
        public async Task<IActionResult> GetAllGoals()
        {
            try
            {
                int userId = GetUserId();
                var goals = await _goalsService.GetAllGoalsAsync(userId);

                //// Encrypt GoalId before returning
                //foreach (var goal in goals)
                //{
                //    goal.GoalId = intEncryptionHelper.EncryptId(goal.GoalId));
                //}

                return Ok(goals);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while retrieving goals.",
                    Details = ex.Message
                });
            }
        }

        // Add a new goal
        [HttpPost("Add_Goal")]
        public async Task<IActionResult> AddGoal([FromBody] Goal goal)
        {
            try
            {
                int userId = GetUserId();
                goal.UserId = userId;

                var result = await _goalsService.AddGoalAsync(goal);
                if (!result.Success)
                {
                    return BadRequest(new { Message = result.Message });
                }

                return CreatedAtAction(nameof(GetAllGoals), new { userId = userId }, goal);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while adding the goal.",
                    Details = ex.Message
                });
            }
        }

        // Delete a goal by encrypted ID
        [HttpDelete("Delete_Goal/{encryptedGoalId}")]
        public async Task<IActionResult> DeleteGoal(string encryptedGoalId)
        {
            try
            {
                int userId = GetUserId();

                // Decrypt the GoalId
                int goalId = EncryptionHelper.DecryptId(encryptedGoalId);
                var goals = await _goalsService.GetAllGoalsAsync(userId);

                var goal = goals.FirstOrDefault(g => g.GoalId == goalId);
                if (goal == null)
                {
                    return NotFound(new { Message = $"Goal with ID {goalId} was not found." });
                }

                // Add logic to delete goal (not implemented in GoalsService yet)
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while deleting the goal.",
                    Details = ex.Message
                });
            }
        }
    }
}
