using FinancialTracker.Domain.Entites;
using FinancialTracker.Domain.ProceduresEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTracker.Domain.Exceptions
{
    public interface ICategories
    {
        Task AddCategoryAsync(Categories category);
        Task EditCategoryAsync(Categories category);
        Task DeleteCategoryAsync(int categoryId);
        Task<Categories> GetCategoryByIdAsync(int categoryId);
        Task<List<CategoriesEntity>> GetAllCategoriesAsync();
    }
}
