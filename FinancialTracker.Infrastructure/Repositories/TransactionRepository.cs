using FinancialTracker.Domain.Interfaces;
using FinancialTracker.Domain.ProceduresEntities;
 using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialTracker.Domain.Entites;
using FinancialTracker.Infrastructure.Utilities;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;
        private readonly ProceduresHelper _helper;
        public TransactionRepository(AppDbContext context,ProceduresHelper helper )
        {
            _context = context;
            _helper = helper;
        }

        public async Task AddTransactionAsync(Transactions transaction)
        {
            try
            {
                if (transaction.Amount <= 0)
                    throw new ValidationException("Transaction amount must be greater than 0.");

                await _context.Transactions.AddAsync(transaction);
                await _context.SaveChangesAsync(); // Or leave this for the caller to handle
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the transaction.", ex);
            }
        }

        public async Task EditTransactionAsync(Transactions transaction)
        {
            try
            {
                var existingTransaction = await _context.Transactions.FindAsync(transaction.TransactionId);
                if (existingTransaction == null)
                {
                    throw new KeyNotFoundException($"Transaction with ID {transaction.TransactionId} not found.");
                }

                existingTransaction.Amount = transaction.Amount;
                existingTransaction.Date = transaction.Date;
                existingTransaction.Notes = transaction.Notes;
                existingTransaction.CategoryId = transaction.CategoryId;
                existingTransaction.IsRecurring = transaction.IsRecurring;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while editing the transaction.", ex);
            }
        }

        public async Task<List<TransactionsEntity>> GetAllTransactionsAsync(int userId)
        {
            try
            {
                // Execute the stored procedure and map the results
                var results = await _helper.ExecuteStoredProcedureAsync(
                    "Get_All_Transactions",
                    reader => new TransactionsEntity
                    {
                        TransactionId = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32(reader.GetOrdinal("Id")),
                        UserId = reader.IsDBNull(reader.GetOrdinal("UserId")) ? 0 : reader.GetInt32(reader.GetOrdinal("UserId")),
                        Amount = reader.IsDBNull(reader.GetOrdinal("Amount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Amount")),
                        Date = reader.IsDBNull(reader.GetOrdinal("Date")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("Date")),
                        Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? string.Empty : reader.GetString(reader.GetOrdinal("Notes")),
                        CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? 0 : reader.GetInt32(reader.GetOrdinal("CategoryId")),
                        IsRecurring = reader.IsDBNull(reader.GetOrdinal("IsRecurring")) ? false : reader.GetBoolean(reader.GetOrdinal("IsRecurring")),
                        CategoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName")) ? string.Empty : reader.GetString(reader.GetOrdinal("CategoryName")),
                        CreateDate = reader.IsDBNull(reader.GetOrdinal("CreateDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("CreateDate")),
                        TypeName = reader.IsDBNull(reader.GetOrdinal("TypeName")) ? string.Empty : reader.GetString(reader.GetOrdinal("TypeName")),
                        Username = reader.IsDBNull(reader.GetOrdinal("Username")) ? string.Empty : reader.GetString(reader.GetOrdinal("Username")),
                    },
                    new SqlParameter("@UserId", userId)  
                );

                return results ?? new List<TransactionsEntity>(); 
            }
            catch (Exception ex)
            {
                // Log or handle exception as needed
                throw new Exception("An error occurred while retrieving all transactions.", ex);
            }
        }
 

        public async Task<Transactions> GetTransactionByIdAsync(int TransactionID)
        {
            if ( string.IsNullOrEmpty(TransactionID.ToString()))
                throw new ValidationException("TransactionID does not exist");

            var Result = await _context.Transactions.FindAsync(TransactionID);
            return Result;
        }

        public async Task<TransactionsEntity> GetTransactionInfoByIdAsync(int transactionId, int userId)
        {
            try
            {

                var results = await _helper.ExecuteStoredProcedureAsync(
                    "GetTransactionById",
                    reader => new TransactionsEntity
                    {
                        TransactionId = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32(reader.GetOrdinal("Id")),
                        UserId = reader.IsDBNull(reader.GetOrdinal("UserId")) ? 0 : reader.GetInt32(reader.GetOrdinal("UserId")),
                        Amount = reader.IsDBNull(reader.GetOrdinal("Amount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Amount")),
                        Date = reader.IsDBNull(reader.GetOrdinal("Date")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("Date")),
                        Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? string.Empty : reader.GetString(reader.GetOrdinal("Notes")),
                        CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? 0 : reader.GetInt32(reader.GetOrdinal("CategoryId")),
                        IsRecurring = reader.IsDBNull(reader.GetOrdinal("IsRecurring")) ? false : reader.GetBoolean(reader.GetOrdinal("IsRecurring")),
                        CategoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName")) ? string.Empty : reader.GetString(reader.GetOrdinal("CategoryName")),
                        CreateDate = reader.IsDBNull(reader.GetOrdinal("CreateDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("CreateDate")),
                        TypeName = reader.IsDBNull(reader.GetOrdinal("TypeName")) ? string.Empty : reader.GetString(reader.GetOrdinal("TypeName")),
                        Username = reader.IsDBNull(reader.GetOrdinal("Username")) ? string.Empty : reader.GetString(reader.GetOrdinal("Username")),
                    },
                    new SqlParameter("@TransactionId", transactionId) // Parameter for stored procedure
                );

                return results?.FirstOrDefault() ?? null; // Return null if no results found
            }
            catch (Exception ex)
            {
                // Log or handle exception as needed
                throw new Exception($"An error occurred while retrieving transaction with ID {transactionId}.", ex);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
