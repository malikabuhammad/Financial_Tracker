using FinancialTracker.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTracker.Infrastructure.MLModels
{
    public class MachineLearningService
    {
        private readonly IWebHostEnvironment _env; 
        private readonly MLContext _mlContext;
        private PredictionEngine<SpendingData, SpendingPrediction> _predictionEngine;

        public MachineLearningService(IWebHostEnvironment env)
        {
            _mlContext = new MLContext();
            _env = env;
            LoadModel();
        }

        private void LoadModel()
        {
            try
            {
                var model = _mlContext.Model.Load(Path.Combine(_env.WebRootPath, "spending-model.zip"), out var inputSchema);
                _predictionEngine = _mlContext.Model.CreatePredictionEngine<SpendingData, SpendingPrediction>(model);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load ML model: {ex.Message}", ex);
            }
        }
        public float PredictSpending(SpendingData input)
        {
            try
            {
                return _predictionEngine.Predict(input).PredictedSpending;
            }
            catch (Exception ex )
            {

                throw new Exception("An error occurred while predicting spending.", ex);
            }
        }



        public void RetrainModel(string trainingDataPath)
        {
            try
            {
                IDataView dataView = _mlContext.Data.LoadFromTextFile<SpendingData>(
                    path: trainingDataPath,
                    hasHeader: true,
                    separatorChar: ','
                    );

                var pipeline = _mlContext.Transforms.Concatenate(
                    "Features",
                    "Month",
                    "CategoryID",
                    "Income",
                    "PreviousSpending",
                    "Savings",
                    "Investments").Append(_mlContext.Regression.Trainers.Sdca(
                      labelColumnName: "CurrentSpending",
                        featureColumnName: "Features"));


                var model = pipeline.Fit(dataView);
                var modelpath = Path.Combine(_env.WebRootPath, "spending-model.zip");
                _mlContext.Model.Save(model, dataView.Schema, modelpath);
                LoadModel();
            }
            catch 
            { 
            
            } 

        }

    }
}
