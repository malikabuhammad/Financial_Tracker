using FinancialTracker.Domain.Interfaces;
using FinancialTracker.Domain.Entities;  

namespace FinancialTracker.Application.Services
{
    public class SpendingPredictionService
    {
        private readonly IMachineLearningService _mlService;

        public SpendingPredictionService(IMachineLearningService mlService)
        {
            _mlService = mlService;
        }

        public async Task<float> PredictUserSpendingAsync(SpendingData input)
        {
            // Call the ML service to predict spending
            return await _mlService.PredictSpendingAsync(input);
        }
    }
}
