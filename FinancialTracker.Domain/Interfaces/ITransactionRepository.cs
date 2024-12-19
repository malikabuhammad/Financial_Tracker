using FinancialTracker.Domain.ProceduresEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FinancialTracker.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<TransactionsEntity>> GetAllTransactionsAsync(int UserID );
        Task<Transaction> GetTransactionByIdAsync(int TransactionID);
        Task<TransactionsEntity> GetTransactionInfoByIdAsync(int TransactionID, int UserID);
        Task AddTransactionAsync(Transaction transaction);
        Task SaveChangesAsync();
    }
}
