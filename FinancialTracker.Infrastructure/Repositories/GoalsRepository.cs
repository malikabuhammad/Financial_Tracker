using FinancialTracker.Domain.Entites;
using FinancialTracker.Domain.Interfaces;
using FinancialTracker.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialTracker.Infrastructure.Repositories
{
    public class GoalsRepository : IGoals
    {
        private readonly AppDbContext _context;
        private readonly ProceduresHelper _helper;

        public GoalsRepository(AppDbContext context, ProceduresHelper helper)
        {
            _context = context;
            _helper = helper;
        }

         public async Task AddGoal(Goal goal)
        {
            await _context.Goals.AddAsync(goal);
            await _context.SaveChangesAsync();
        }

         public async Task EditGoal(Goal goal)
        {
            try
            {
                var existingGoal = await _context.Goals.FindAsync(goal.GoalId);
                if (existingGoal == null)
                {
                    throw new KeyNotFoundException($"Goal with ID {goal.GoalId} not found.");
                }

                existingGoal.DueDate = goal.DueDate;
                existingGoal.SavedAmount = goal.SavedAmount;
                existingGoal.Name = goal.Name;
                existingGoal.TargetAmount = goal.TargetAmount;

                _context.Goals.Update(existingGoal);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                 throw new InvalidOperationException("An error occurred while editing the goal.", ex);
            }
        }

         public async Task<List<Goal>> GetAllGoals(int userId)
        {
            try
            {
                return await _context.Goals
                    .Where(goal => goal.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                 throw new InvalidOperationException("An error occurred while retrieving goals.", ex);
            }
        }

         public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
