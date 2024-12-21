using FinancialTracker.Domain.Entites;
using FinancialTracker.Domain.ProceduresEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTracker.Domain.Interfaces
{
    public interface IGoals
    {
        Task<List<Goal>> GetAllGoals(int UserID);
        Task AddGoal(Goal goal);
        Task EditGoal(Goal goal);
        Task SaveChangesAsync();
    }
}
