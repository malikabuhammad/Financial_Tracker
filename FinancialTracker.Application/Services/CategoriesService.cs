using FinancialTracker.Domain.Entites;
using FinancialTracker.Domain.Entities;
using FinancialTracker.Domain.Exceptions;
using FinancialTracker.Domain.Interfaces;
using FinancialTracker.Domain.ProceduresEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialTracker.Application.Services
{
    public class CategoriesService
    {
        private readonly ICategories _categoriesRepository;

        public CategoriesService(ICategories categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        public async Task<(bool Success, string Message)> AddCategoryAsync(Categories category)
        {
            try
            {
                await _categoriesRepository.AddCategoryAsync(category);
                return (true, "Category added successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred while adding the category: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> EditCategoryAsync(Categories category)
        {
            try
            {
                await _categoriesRepository.EditCategoryAsync(category);
                return (true, "Category updated successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return (false, ex.Message);
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred while updating the category: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> DeleteCategoryAsync(int categoryId)
        {
            try
            {
                await _categoriesRepository.DeleteCategoryAsync(categoryId);
                return (true, "Category deleted successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return (false, ex.Message);
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred while deleting the category: {ex.Message}");
            }
        }

        public async Task<Categories> GetCategoryByIdAsync(int categoryId)
        {
            return await _categoriesRepository.GetCategoryByIdAsync(categoryId);
        }

        public async Task<List<CategoriesEntity>> GetAllCategoriesAsync()
        {
            return await _categoriesRepository.GetAllCategoriesAsync();
        }
    }
}
