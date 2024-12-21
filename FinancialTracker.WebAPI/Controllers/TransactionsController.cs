using FinancialTracker.Application.DTOs;
using FinancialTracker.Application.Services;
using FinancialTracker.Domain.Entites;
using FinancialTracker.Infrastructure.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using static FinancialTracker.Application.DTOs.TransactionsDTOs;

namespace FinancialTracker.WebAPI.Controllers
{
    [Authorize]
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

                 var TransData = await _transactionService.GetAllTransactionsAsync(userId);
                var result = TransData
                    .Select(r => new { 
                        EncryptID= EncryptionHelper.EncryptId( r.TransactionId),
                        r.TypeName,
                        r.Username,
                        r.Date,
                        r.CreateDate,
                        r.Notes,
                        r.CategoryName,
                        r.CategoryId,
                        r.Amount,
                        r.IsRecurring,

                
                    });
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

        [HttpGet("Get_Transaction_Details/{EncryptedID}")]
        public async Task<IActionResult> GetTransactionById(string EncryptedID)
        {
            try
            {
                int userId = GetUserId();

                int TransactionID = EncryptionHelper.DecryptId(EncryptedID);
                var transaction = await _transactionService.GetTransactionByIdAsync(TransactionID);
                if (transaction == null)
                {
                    return NotFound(new { Message = $"Transaction with ID {TransactionID} was not found." });
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
        public async Task<IActionResult> AddTransaction([FromBody] TransactionAddModel transaction)
        {
            try
            {
                int userId = GetUserId();

                Transactions transactionObj = new Transactions()
                {Amount= transaction.Amount,
                CategoryId= transaction.CategoryId,
                CreateDate=DateTime.Now,
                Date=transaction.Date,
                IsRecurring=transaction.IsRecurring,
                Notes=transaction.Notes,

                 
                 UserId=userId,
                };

                await _transactionService.AddTransactionAsync(transactionObj);
                return Ok(200);
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
        public async Task<IActionResult> EditTransaction([FromBody] TransactionEditModel transaction)
        {
            try
            {
              var TransObj =  await _transactionService.GetTransactionByIdAsync(transaction.TransactionId);
 

                TransObj.CategoryId = transaction.CategoryId;
                TransObj.Amount = transaction.Amount;
                TransObj.Date = transaction.Date;
                TransObj.Notes = transaction.Notes;
                TransObj.IsRecurring = transaction.IsRecurring;
                TransObj.TransactionId = transaction.TransactionId;
    
               
                 await _transactionService.EditTransactionAsync(TransObj);
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
