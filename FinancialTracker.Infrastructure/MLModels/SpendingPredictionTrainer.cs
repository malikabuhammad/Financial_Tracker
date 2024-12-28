using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.IO;

namespace FinancialTracker.Infrastructure.MLModels
{
    public class SpendingPredictionTrainer
    {
        private readonly string _csvFilePath;
        private readonly string _modelPath;
        private readonly MLContext _mlContext;

        public SpendingPredictionTrainer(string csvFilePath, string modelPath)
        {
            _csvFilePath = csvFilePath;
            _modelPath = modelPath;
            _mlContext = new MLContext();
        }

        public void TrainModel()
        {
            try
            {
                // Step 1: Load the training data
                Console.WriteLine("Loading training data...");
                IDataView dataView = _mlContext.Data.LoadFromTextFile<SpendingData>(
                    path: _csvFilePath,
                    hasHeader: true,
                    separatorChar: ',');

                // Step 2: Define the data preparation and training pipeline
                Console.WriteLine("Defining training pipeline...");
                var pipeline = _mlContext.Transforms.Concatenate(
                        "Features",
                        nameof(SpendingData.Month),
                        nameof(SpendingData.CategoryId),
                        nameof(SpendingData.Income),
                        nameof(SpendingData.PreviousSpending),
                        nameof(SpendingData.Savings),
                        nameof(SpendingData.Investments))
                    .Append(_mlContext.Transforms.NormalizeMeanVariance("Features")) // Normalize features
                    .Append(_mlContext.Regression.Trainers.Sdca(
                        labelColumnName: "CurrentSpending", // The label to predict
                        featureColumnName: "Features"));

                // Step 3: Train the model
                Console.WriteLine("Training the model...");
                var model = pipeline.Fit(dataView);

                // Step 4: Save the trained model to a file
                Console.WriteLine($"Saving the model to {_modelPath}...");
                _mlContext.Model.Save(model, dataView.Schema, _modelPath);

                Console.WriteLine("Model training and saving complete!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during model training: {ex.Message}");
                throw;
            }
        }
    }

    // Training data schema
    public class SpendingData
    {
        [LoadColumn(0)] public float Month { get; set; }
        [LoadColumn(1)] public float CategoryId { get; set; }
        [LoadColumn(2)] public float Income { get; set; }
        [LoadColumn(3)] public float PreviousSpending { get; set; }
        [LoadColumn(4)] public float Savings { get; set; }
        [LoadColumn(5)] public float Investments { get; set; }
        [LoadColumn(6)] public float CurrentSpending { get; set; } // Label to predict
    }
}
