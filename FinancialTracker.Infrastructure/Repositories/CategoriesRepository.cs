using FinancialTracker.Domain.Entites;
using FinancialTracker.Domain.Entities;
using FinancialTracker.Domain.Exceptions;
using FinancialTracker.Domain.Interfaces;
using FinancialTracker.Domain.ProceduresEntities;
using FinancialTracker.Infrastructure.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialTracker.Infrastructure.Repositories
{
    public class CategoriesRepository : ICategories
    {
        private readonly AppDbContext _context;
        private readonly ProceduresHelper _helper;

        public CategoriesRepository(AppDbContext context, ProceduresHelper helper)
        {
            _context = context;
            _helper = helper;
        }

        public async Task AddCategoryAsync(Categories Categories)
        {
            await _context.Categories.AddAsync(Categories);
            await _context.SaveChangesAsync();
        }

        public async Task EditCategoryAsync(Categories Categories)
        {
            var existingCategories = await _context.Categories.FindAsync(Categories.CategoryId);
            if (existingCategories == null)
                throw new KeyNotFoundException($"Categories with ID {Categories.CategoryId} not found.");

            existingCategories.Name = Categories.Name;
            existingCategories.TypeId = Categories.TypeId;
            existingCategories.UpdatedAt = Categories.UpdatedAt;

            _context.Categories.Update(existingCategories);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int CategoryId)
        {
            var Categories = await _context.Categories.FindAsync(CategoryId);
            if (Categories == null)
                throw new KeyNotFoundException($"Categories with ID {CategoryId} not found.");

            _context.Categories.Remove(Categories);
            await _context.SaveChangesAsync();
        }

        public async Task<Categories> GetCategoryByIdAsync(int CategoryId)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == CategoryId );
        }

 

        public async Task<List<CategoriesEntity>> GetAllCategoriesAsync()
        {

            try
            {
                // Execute the stored procedure and map the results
                var results = await _helper.ExecuteStoredProcedureAsync(
                    "Get_All_Categories",
                    reader => new CategoriesEntity
                    {
                         TypeId = reader.IsDBNull(reader.GetOrdinal("TypeId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TypeId")),
                           CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? 0 : reader.GetInt32(reader.GetOrdinal("CategoryId")),
                         CategoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName")) ? string.Empty : reader.GetString(reader.GetOrdinal("CategoryName")),
                         TypeName = reader.IsDBNull(reader.GetOrdinal("TypeName")) ? string.Empty : reader.GetString(reader.GetOrdinal("TypeName")),
                     }
                 );

                return results ?? new List<CategoriesEntity>();
            }
            catch (Exception ex)
            {
                // Log or handle exception as needed
                throw new Exception("An error occurred while retrieving all transactions.", ex);
            }
        }
    }
}
