using FinancialTracker.Application.Services;
using FinancialTracker.Domain.Entites;
using FinancialTracker.Domain.Entities;
using FinancialTracker.Domain.Interfaces;
using FinancialTracker.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/spending-predictions")]
public class SpendingPredictionController : BaseController
{
    private readonly SpendingPredictionService _predictionService;
    private readonly ITransactionRepository _transactionRepository;

    public SpendingPredictionController(
        SpendingPredictionService predictionService,
        ITransactionRepository transactionRepository)
    {
        _predictionService = predictionService;
        _transactionRepository = transactionRepository;
    }

    [HttpPost]
    public async Task<IActionResult> PredictSpending()
    {
        try
        {
            // Step 1: Get the logged-in user ID
            int userId = GetUserId(); // Assume this retrieves the user ID from the JWT token

            // Step 2: Fetch data from the database for the current user
            var userTransactions = await _transactionRepository.GetAllTransactionsAsync(userId);

            // Step 3: Calculate totals for SpendingData
            var income = userTransactions.Where(t => t.TypeId == 1).Sum(t => t.Amount); // Income
            var previousSpending = userTransactions.Where(t => t.TypeId == 2).Sum(t => t.Amount); // Expenses
            var savings = userTransactions.Where(t => t.TypeId == 3).Sum(t => t.Amount); // Savings
            var investments = userTransactions.Where(t => t.TypeId == 4).Sum(t => t.Amount); // Investments

            // Step 4: Create SpendingData object
            var spendingData = new SpendingData
            {
                Month = DateTime.Now.Month,
                CategoryId = 1, 
                Income = (float)income,
                PreviousSpending = (float)previousSpending,
 
            };

            // Step 5: Call the prediction service
            var predictedSpending = await _predictionService.PredictUserSpendingAsync(spendingData);

            // Step 6: Return the prediction
            return Ok(new { PredictedSpending = predictedSpending });
        }
        catch (Exception ex)
        {
            // Handle errors
            return StatusCode(500, new { Message = "An error occurred while predicting spending.", Error = ex.Message });
        }
    }

 
}
