using FinancialTracker.Application.DTOs;
using FinancialTracker.Application.Services;
using FinancialTracker.Domain.Entites;
using FinancialTracker.Infrastructure.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using static FinancialTracker.Application.DTOs.RecurringTransactionDto;

namespace FinancialTracker.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RecurringTransactionsController : BaseController
    {
        private readonly RecurringTransactionsService _service;

        public RecurringTransactionsController(RecurringTransactionsService service)
        {
            _service = service;
        }

        [HttpGet("Get_All_Recurring_Transactions")]
        public async Task<IActionResult> GetAllRecurringTransactions()
        {
            try
            {
                int userId = GetUserId();

                var transactions = await _service.GetAllRecurringTransactionsAsync(userId);
                var result = transactions.Select(r => new
                {
                    EncryptedId = EncryptionHelper.EncryptId(r.RecurringTransactionId),
                    r.Amount,
                    r.CategoryID,
                    r.CategoryName,
                    r.FrequencyID,
                    r.StartDate,
                    r.EndDate,
                    r.Notes,
                    r.IsAtive,
                    r.IsOneTime
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while retrieving recurring transactions.",
                    Details = ex.Message
                });
            }
        }

        [HttpGet("Get_Recurring_Transaction_Details/{EncryptedId}")]
        public async Task<IActionResult> GetRecurringTransactionById(string EncryptedId)
        {
            try
            {
                int userId = GetUserId();
                int transactionId = EncryptionHelper.DecryptId(EncryptedId);

                var transaction = await _service.GetRecurringTransactionByIdAsync(transactionId, userId);
                if (transaction == null)
                {
                    return NotFound(new { Message = $"Recurring transaction with ID {EncryptedId} not found." });
                }

                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while retrieving the recurring transaction.",
                    Details = ex.Message
                });
            }
        }

        [HttpPost("Add_Recurring_Transaction")]
        public async Task<IActionResult> AddRecurringTransaction([FromBody] RecurringTransactionAddModel transaction)
        {
            try
            {
                int userId = GetUserId();
                var recurringTransaction = new RecurringTransaction
                {
                    UserId = userId,
                    CategoryId = transaction.CategoryId,
                    Amount = transaction.Amount,
                    FrequencyID = transaction.FrequencyID,
                    StartDate = transaction.StartDate,
                    EndDate = transaction.EndDate,
                    Notes = transaction.Notes,
                    IsOneTime = transaction.IsOneTime
                };

                var id = await _service.AddRecurringTransactionAsync(recurringTransaction);
                return CreatedAtAction(nameof(GetRecurringTransactionById), new { EncryptedId = EncryptionHelper.EncryptId(id) }, transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while adding the recurring transaction.",
                    Details = ex.Message
                });
            }
        }

        [HttpPut("Update_Recurring_Transaction")]
        public async Task<IActionResult> EditRecurringTransaction([FromBody] RecurringTransactionEditModel transaction)
        {
            try
            {
                int userId = GetUserId();

                var recurringTransaction = await _service.GetRecurringTransactionByIdAsync(transaction.RecurringTransactionId, userId);
                if (recurringTransaction == null)
                {
                    return NotFound(new { Message = "Recurring transaction not found." });
                }

                recurringTransaction.CategoryId = transaction.CategoryId;
                recurringTransaction.Amount = transaction.Amount;
                recurringTransaction.FrequencyID = transaction.FrequencyID;
                recurringTransaction.StartDate = transaction.StartDate;
                recurringTransaction.EndDate = transaction.EndDate;
                recurringTransaction.Notes = transaction.Notes;
                recurringTransaction.IsOneTime = transaction.IsOneTime;
                recurringTransaction.IsActive = transaction.IsActive;
                recurringTransaction.CancelledDate = transaction.CancelledDate;
                recurringTransaction.CancelReason = transaction.CancelReason;
                recurringTransaction.NextRunDate = transaction.NextRunDate;

                await _service.EditRecurringTransactionAsync(recurringTransaction);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while updating the recurring transaction.",
                    Details = ex.Message
                });
            }
        }
    }
}
