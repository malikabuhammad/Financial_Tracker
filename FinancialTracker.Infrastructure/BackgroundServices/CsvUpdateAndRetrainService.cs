using FinancialTracker.Infrastructure.Repositories;
using FinancialTracker.Infrastructure.MLModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FinancialTracker.Infrastructure.BackgroundServices
{
    public class CsvUpdateAndRetrainService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _csvFilePath;

        public CsvUpdateAndRetrainService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            // Define the CSV file path
            _csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Infrastructure", "MLModels", "spending-data.csv");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Schedule the service to run daily at midnight
            var firstRun = DateTime.Today.AddDays(1).AddHours(0); // Midnight tomorrow
            var timeUntilFirstRun = firstRun - DateTime.Now;

            // Set up the timer to run daily
            //_timer = new Timer(PerformCsvUpdateAndRetrain, null, timeUntilFirstRun, TimeSpan.FromDays(1));
            _timer = new Timer(PerformCsvUpdateAndRetrain, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private async void PerformCsvUpdateAndRetrain(object state)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    // Resolve scoped dependencies within the scope
                    var exportService = scope.ServiceProvider.GetRequiredService<SpendingDataExportService>();
                    var mlService = scope.ServiceProvider.GetRequiredService<MachineLearningService>();

                    // Step 1: Export the latest spending data to CSV
                    Console.WriteLine("Updating CSV with the latest data...");
                    await exportService.ExportSpendingDataToCsvAsync(_csvFilePath);
                    Console.WriteLine($"CSV updated at: {_csvFilePath}");

                    // Step 2: Retrain the ML.NET model with the updated CSV
                    Console.WriteLine("Retraining the ML.NET model...");
                    mlService.RetrainModel(_csvFilePath);
                    Console.WriteLine("Model retrained successfully!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during CSV update and retrain: {ex.Message}");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
