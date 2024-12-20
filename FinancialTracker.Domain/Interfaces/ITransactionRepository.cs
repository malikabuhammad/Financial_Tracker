using FinancialTracker.Domain.Entites;
using FinancialTracker.Domain.ProceduresEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace FinancialTracker.Domain.Interfaces
{
    //فكرة عامة 
    public interface ITransactionRepository
    {
        Task<List<TransactionsEntity>> GetAllTransactionsAsync(int UserID );
        Task<Transactions> GetTransactionByIdAsync(int TransactionID);
        Task<TransactionsEntity> GetTransactionInfoByIdAsync(int TransactionID, int UserID);
        Task AddTransactionAsync(Transactions transaction);
        Task EditTransactionAsync(Transactions transaction);
       
         Task SaveChangesAsync();
    }
}
