using FinancialTracker.Domain.Entites;
using FinancialTracker.Domain.Interfaces;
using FinancialTracker.Domain.ProceduresEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FinancialTracker.Application.Services
{
    public class TransactionsService
    {
        public readonly ITransactionRepository _transactionRepository;
        public TransactionsService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }


        public async Task<List<TransactionsEntity>> GetAllTransactionsAsync(int userId)
        {
            return await _transactionRepository.GetAllTransactionsAsync(userId);
        }

        public async Task<TransactionsEntity> GetTransactionInfoByIdAsync(int transactionId , int userId)
        {
            return await _transactionRepository.GetTransactionInfoByIdAsync(transactionId, userId);
        }
        public async Task<Transactions> GetTransactionByIdAsync(int TransactionID)
        {
            return await _transactionRepository.GetTransactionByIdAsync(TransactionID);

        }



        public async Task AddTransactionAsync(Transactions transaction)
        {
            await _transactionRepository.AddTransactionAsync(transaction);
            await _transactionRepository.SaveChangesAsync();
        }

        public async Task EditTransactionAsync(Transactions transaction)
        {
            await _transactionRepository.EditTransactionAsync(transaction);
            await _transactionRepository.SaveChangesAsync();
        }
    }
}
 