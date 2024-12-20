using FinancialTracker.Application.Services;
using FinancialTracker.Domain.Entites;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class TransactionsController : BaseController 
    {
        private readonly TransactionsService _transactionService;

        public TransactionsController(TransactionsService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("Get_All_Transactions")]
        public async Task<ActionResult> GetAllTransactions()
        {
            try
            {
                 int userId = GetUserId();

                 var result = await _transactionService.GetAllTransactionsAsync(userId);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while retrieving transactions.",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("Get_Transaction_Details/{TransactionId}")]
        public async Task<IActionResult> GetTransactionById(int TransactionId)
        {
            try
            {
                int userId = GetUserId(); 

               
                var transaction = await _transactionService.GetTransactionByIdAsync(TransactionId);
                if (transaction == null)
                {
                    return NotFound(new { Message = $"Transaction with ID {TransactionId} was not found." });
                }

                return Ok(transaction);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while retrieving the transaction.",
                    Details = ex.Message
                });
            }
        }

        [HttpPost("Add_Transaction")]
        public async Task<IActionResult> AddTransaction([FromBody] Transactions transaction)
        {
            try
            {
                int userId = GetUserId();
                transaction.UserId = userId;  

                await _transactionService.AddTransactionAsync(transaction);
                return CreatedAtAction(nameof(GetTransactionById), new { transactionId = transaction.TransactionId }, transaction);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Message = "An error occurred while adding the transaction.",
                    Details = ex.Message
                });
            }
        }

        [HttpPut("Update_Transaction")]
        public async Task<IActionResult> EditTransaction([FromBody] Transactions transaction)
        {
            try
            {
                int userId = GetUserId();  

                await _transactionService.EditTransactionAsync(transaction);
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
                    Message = "An error occurred while updating the transaction.",
                    Details = ex.Message
                });
            }
        }

        [HttpDelete("Delete_Transaction/{TransactionID}")]
        public async Task<IActionResult> DeleteTransaction(int TransactionID)
        {
            try
            {
                int userId = GetUserId(); 

                var transaction = await _transactionService.GetTransactionByIdAsync(TransactionID);
                if (transaction == null)
                {
                    return NotFound(new { Message = $"Transaction with ID {TransactionID} was not found." });
                }

                //await _transactionService.DeleteTransactionAsync(transaction);
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
                    Message = "An error occurred while deleting the transaction.",
                    Details = ex.Message
                });
            }
        }
    }
}
