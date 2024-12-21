using FinancialTracker.Domain.Entites;
using FinancialTracker.Domain.Entities;
using FinancialTracker.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialTracker.Application.Services
{
    public class GoalsService
    {
        private readonly IGoals _goalsRepository;

        public GoalsService(IGoals goalsRepository)
        {
            _goalsRepository = goalsRepository;
        }

 
        public async Task<(bool Success, string Message)> AddGoalAsync(Goal goal)
        {
            try
            {
                await _goalsRepository.AddGoal(goal);
                return (true, "Goal added successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred while adding the goal: {ex.Message}");
            }
        }

 
        public async Task<(bool Success, string Message)> EditGoalAsync(Goal goal)
        {
            try
            {
                await _goalsRepository.EditGoal(goal);
                return (true, "Goal updated successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return (false, ex.Message);
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred while updating the goal: {ex.Message}");
            }
        }

 
        public async Task<List<Goal>> GetAllGoalsAsync(int userId)
        {
            try
            {
                return await _goalsRepository.GetAllGoals(userId);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while fetching goals: {ex.Message}");
            }
        }
    }
}
