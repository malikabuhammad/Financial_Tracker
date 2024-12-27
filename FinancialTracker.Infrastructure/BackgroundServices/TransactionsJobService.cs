using FinancialTracker.Domain.Interfaces;
using FinancialTracker.Infrastructure.Repositories;
using FinancialTracker.Infrastructure.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FinancialTracker.Infrastructure.BackgroundServices
{
    public class TransactionsJobService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;

        public TransactionsJobService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
             var firstRun = DateTime.Today.AddDays(1).AddHours(0); // Midnight tomorrow
            var timeUntilFirstRun = firstRun - DateTime.Now;

            // Schedule the timer to run  
            //_timer = new Timer(RunDailyJobs, null, timeUntilFirstRun, TimeSpan.FromDays(1));
            _timer = new Timer(RunDailyJobs, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private async void RunDailyJobs(object state)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                     var proceduresHelper = scope.ServiceProvider.GetRequiredService<ProceduresHelper>();
                    var jobRepository = scope.ServiceProvider.GetRequiredService<IJobRepository>();

                     await jobRepository.ProcessRecurringTransactionsAsync(proceduresHelper);

               
                    var userIds = await GetAllUserIds(scope);
                    foreach (var userId in userIds)
                    {
                        await jobRepository.AnalyzeFinancialStatusAsync(userId, proceduresHelper);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in TransactionsJobService: {ex.Message}");
            }
        }

        private async Task<int[]> GetAllUserIds(IServiceScope scope)
        {
            // Resolve IUserRepository and fetch user IDs
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            return userRepository.GetAllUsersAsync().Result.ToList().Select(r => r.UserId).ToArray();

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
