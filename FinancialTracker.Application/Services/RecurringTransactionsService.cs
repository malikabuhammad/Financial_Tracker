using FinancialTracker.Domain.Entites;
using FinancialTracker.Domain.Interfaces;
using FinancialTracker.Domain.ProceduresEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialTracker.Application.Services
{
    public class RecurringTransactionsService
    {
        private readonly IRecurringTransactionRepository _repository;

        public RecurringTransactionsService(IRecurringTransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RecurringTransactionsEntity>> GetAllRecurringTransactionsAsync(int userId)
        {
            return await _repository.GetAllRecurringTransactionsAsync(userId);
        }

        public async Task<RecurringTransaction> GetRecurringTransactionByIdAsync(int transactionId, int userId)
        {
            return await _repository.GetReccuringTransactionInfoByIdAsync(transactionId, userId);
        }

        public async Task<int> AddRecurringTransactionAsync(RecurringTransaction transaction)
        {
            return await _repository.AddRecurringTransactionAsync(transaction);
        }

        public async Task<int> EditRecurringTransactionAsync(RecurringTransaction transaction)
        {
            return await _repository.EditRecurringTransactionAsync(transaction);
        }
    }
}
