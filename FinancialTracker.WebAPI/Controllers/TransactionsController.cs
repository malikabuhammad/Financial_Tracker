using FinancialTracker.Application.Services;
using FinancialTracker.Domain.Entites;
using FinancialTracker.Domain.ProceduresEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTracker.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {

        private readonly TransactionsService _transactionService; 

        public TransactionsController(TransactionsService transactionService)
        {
            _transactionService = transactionService;
        }


        [HttpGet("")]
        public async Task<ActionResult> GetAllTransactions(int UserID)
        {
            try
            {
                var result = _transactionService.GetAllTransactionsAsync(UserID);
                return Ok(result);
            }
            catch (Exception ex )
            {

                throw;
            }
         
        }

        [HttpGet("get_trans_details/{TransactionId}")]
        public async Task<IActionResult> GetTransactionById(int TransactionId)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(TransactionId);
            if (transaction == null) return NotFound();
            return Ok(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> AddTransaction([FromBody] Transactions transaction)
        {
            await _transactionService.AddTransactionAsync(transaction);
            return CreatedAtAction(nameof(GetTransactionById), new { transactionId = transaction.TransactionId }, transaction);
        }

        [HttpPut]
        public async Task<IActionResult> EditTransaction([FromBody] Transactions transaction)
        {
            await _transactionService.EditTransactionAsync(transaction);
            return NoContent();
        }
    }
}
 