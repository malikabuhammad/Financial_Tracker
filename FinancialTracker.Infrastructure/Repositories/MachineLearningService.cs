using Microsoft.ML;
using FinancialTracker.Domain.Interfaces;
using FinancialTracker.Domain.Entities; // Adjust to your SpendingData entity namespace

namespace FinancialTracker.Infrastructure.Services
{
    public class MachineLearningService : Domain.Interfaces.IMachineLearningService
    {
        private readonly string _modelPath = "Path/To/Your/spending-model.zip"; // Path to the ML.NET model
        private readonly MLContext _mlContext;
        private PredictionEngine<SpendingData, SpendingPrediction> _predictionEngine;

        public MachineLearningService()
        {
            _mlContext = new MLContext();
            LoadModel();
        }

        private void LoadModel()
        {
            // Load the ML.NET model
            var model = _mlContext.Model.Load(_modelPath, out var inputSchema);
            _predictionEngine = _mlContext.Model.CreatePredictionEngine<SpendingData, SpendingPrediction>(model);
        }

        public Task<float> PredictSpendingAsync(SpendingData input)
        {
            var prediction = _predictionEngine.Predict(input);
            return Task.FromResult(prediction.PredictedSpending);
        }
    }
}
