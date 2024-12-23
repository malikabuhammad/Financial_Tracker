using FinancialTracker.Domain.Entites;
using FinancialTracker.Domain.ProceduresEntities;
 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTracker.Domain.Interfaces
{
    public  interface IRecurringTransactionRepository
    {

        Task<List<RecurringTransactionsEntity>> GetAllRecurringTransactionsAsync(int UserID);
        Task<RecurringTransactionsEntity> GetAllRecurringTransactionByIdAsync(int TransactionID);
        Task<RecurringTransaction> GetReccuringTransactionInfoByIdAsync(int TransactionID, int UserID);
        Task <int>AddRecurringTransactionAsync(RecurringTransaction transaction);
        Task <int> EditRecurringTransactionAsync(RecurringTransaction transaction);

        Task SaveChangesAsync();
    }
}
