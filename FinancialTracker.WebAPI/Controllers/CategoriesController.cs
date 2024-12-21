using FinancialTracker.Application.Services;
using FinancialTracker.Domain.Entites;
using FinancialTracker.Domain.Entities;
using FinancialTracker.Infrastructure.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FinancialTracker.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoriesService _categoriesService;

        public CategoriesController(CategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpGet("GetCategoryById/{encryptedId}")]
        public async Task<IActionResult> GetCategoryById(string encryptedId)
        {
            try
            {
              
                int categoryId = EncryptionHelper.DecryptId(encryptedId);

               
            

            
                var category = await _categoriesService.GetCategoryByIdAsync(categoryId);
                if (category == null)
                {
                    return NotFound(new { Message = $"Category with ID {encryptedId} was not found." });
                }

                return Ok(category);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while retrieving the category.",
                    Details = ex.Message
                });
            }
        }

        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] Categories category)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("sub")?.Value); // Extract user ID from token
                category.UserId = userId;

                await _categoriesService.AddCategoryAsync(category);
                return Ok(new { Message = "Category added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error adding category", Details = ex.Message });
            }
        }

        [HttpPut("EditCategory/{encryptedId}")]
        public async Task<IActionResult> EditCategory(string encryptedId, [FromBody] Categories category)
        {
            try
            {
                
                int categoryId = EncryptionHelper.DecryptId(encryptedId);

                  category.CategoryId = categoryId;
                  

                await _categoriesService.EditCategoryAsync(category);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error editing category", Details = ex.Message });
            }
        }


        [HttpGet("Get_All_Categories")]
        public async Task<ActionResult> GetAllCategories()
        {
            try
            {
         
               
                var categories = await _categoriesService.GetAllCategoriesAsync();

                 
                var encryptedResult = categories.Select(c => new
                {
                    EncryptedId = EncryptionHelper.EncryptId(c.CategoryId), 
                    c.TypeId,
                    c.CategoryName,
                    c.TypeName,
                
                });

                return Ok(encryptedResult);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while retrieving categories.",
                    Details = ex.Message
                });
            }
        }

    }
}
