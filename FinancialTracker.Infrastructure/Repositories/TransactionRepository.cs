using FinancialTracker.Domain.Interfaces;
using FinancialTracker.Domain.ProceduresEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FinancialTracker.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository

    {
        public Task AddTransactionAsync(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public Task<List<TransactionsEntity>> GetAllTransactionsAsync(int UserID)
        {
            throw new NotImplementedException();
        }

        public Task<Transaction> GetTransactionByIdAsync(int TransactionID)
        {
            throw new NotImplementedException();
        }

        public Task<TransactionsEntity> GetTransactionInfoByIdAsync(int TransactionID, int UserID)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
