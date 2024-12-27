using FinancialTracker.Infrastructure.Utilities;
using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;

namespace FinancialTracker.Infrastructure.Repositories
{
    public interface IJobRepository
    {
        Task ProcessRecurringTransactionsAsync(ProceduresHelper helper);
        Task AnalyzeFinancialStatusAsync(int userId, ProceduresHelper helper);
    }

    public class JobRepository : IJobRepository
    {
        public Task ProcessRecurringTransactionsAsync(ProceduresHelper helper)
        {
            try
            {
                // Use ProceduresHelper to execute the stored procedure
                helper.ExecuteStoredProcedureAsync<object>("ProcessRecurringTransactions", reader => null);
                Console.WriteLine("Processed recurring transactions.");
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing recurring transactions.", ex);
            }
        }

        public Task AnalyzeFinancialStatusAsync(int userId, ProceduresHelper helper)
        {
            try
            {
                // Use ProceduresHelper to execute the stored procedure
                helper.ExecuteStoredProcedureAsync<object>(
                    "AnalyzeFinancialStatus",
                    reader => null,
                    new Microsoft.Data.SqlClient.SqlParameter("@UserId", userId)
                );
                Console.WriteLine($"Analyzed financial status for user {userId}.");
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while analyzing financial status.", ex);
            }
        }
    }
}
