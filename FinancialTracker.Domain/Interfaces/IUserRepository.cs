using FinancialTracker.Domain.Entites;
using FinancialTracker.Domain.Entities;
using System.Threading.Tasks;

namespace FinancialTracker.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<Users> GetUserByIdAsync(int userId);
        Task<Users> GetUserByUsernameAsync(string username);
        Task<List<Users>> GetAllUsersAsync();  
        Task AddUserAsync(Users user);
        Task UpdateUserAsync(Users user);
        Task DeleteUserAsync(Users user);  
        Task SaveChangesAsync();
    }
}
