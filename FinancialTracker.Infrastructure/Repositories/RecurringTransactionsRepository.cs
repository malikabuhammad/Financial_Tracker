using FinancialTracker.Domain.Entites;
using FinancialTracker.Domain.Entities;
using FinancialTracker.Domain.Interfaces;
using FinancialTracker.Domain.ProceduresEntities;
using FinancialTracker.Infrastructure.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FinancialTracker.Infrastructure.Repositories
{
    public class RecurringTransactionsRepository : IRecurringTransactionRepository
    {
        private readonly ProceduresHelper _helper;
        private readonly AppDbContext _context;

        public RecurringTransactionsRepository(ProceduresHelper helper, AppDbContext context)
        {
            _helper = helper;
            _context = context;
        }

        public async Task<int> AddRecurringTransactionAsync(RecurringTransaction transaction)
        {
            try
            {
                var recurringTransactionId = await _helper.ExecuteScalarStoredProcedureAsync<int>(
                    "AddRecurringTransaction",
                    new SqlParameter("@UserId", transaction.UserId),
                    new SqlParameter("@CategoryId", transaction.CategoryId),
                    new SqlParameter("@Amount", transaction.Amount),
                    new SqlParameter("@FrequencyID", transaction.FrequencyID),
                    new SqlParameter("@StartDate", transaction.StartDate),
                    new SqlParameter("@EndDate", transaction.EndDate ?? (object)DBNull.Value),
                    new SqlParameter("@Notes", transaction.Notes ?? (object)DBNull.Value),
                    new SqlParameter("@IsOneTime", transaction.IsOneTime)
                );

                return recurringTransactionId;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding recurring transaction.", ex);
            }
        }

        public async Task<int> EditRecurringTransactionAsync(RecurringTransaction transaction)
        {
            try
            {
               var value= await _helper.ExecuteScalarStoredProcedureAsync<int>(
                    "EditRecurringTransaction",
                    new SqlParameter("@RecurringTransactionId", transaction.RecurringTransactionId),
                    new SqlParameter("@UserId", transaction.UserId),
                    new SqlParameter("@CategoryId", transaction.CategoryId),
                    new SqlParameter("@Amount", transaction.Amount),
                    new SqlParameter("@FrequencyID", transaction.FrequencyID),
                    new SqlParameter("@StartDate", transaction.StartDate),
                    new SqlParameter("@EndDate", transaction.EndDate ?? (object)DBNull.Value),
                    new SqlParameter("@Notes", transaction.Notes ?? (object)DBNull.Value),
                    new SqlParameter("@IsOneTime", transaction.IsOneTime),
                    new SqlParameter("@IsActive", transaction.IsActive)
                );
                return value;
            }
            catch (Exception ex)
            {
                // Handle/log exception here
                throw new Exception("Error editing recurring transaction", ex);
            }
        }

        public async Task<RecurringTransactionsEntity> GetAllRecurringTransactionByIdAsync(int TransactionID)
        {
            try
            {
                var results = await _helper.ExecuteStoredProcedureAsync(
                    "GetRecurringTransactionById",
                    reader => new RecurringTransactionsEntity
                    {
                        RecurringTransactionId = reader.GetInt32(reader.GetOrdinal("RecurringTransactionId")),
                        UserID = reader.GetInt32(reader.GetOrdinal("UserId")),
                        CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                        Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                        FrequencyID = reader.GetInt32(reader.GetOrdinal("FrequencyID")),
                        StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                        EndDate = reader.IsDBNull(reader.GetOrdinal("EndDate")) ? null : reader.GetDateTime(reader.GetOrdinal("EndDate")),
                        NextRunDate = reader.GetDateTime(reader.GetOrdinal("NextRunDate")),
                        Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? string.Empty : reader.GetString(reader.GetOrdinal("Notes")),
                        IsAtive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                        IsOneTime = reader.GetBoolean(reader.GetOrdinal("IsOneTime"))
                    },
                    new SqlParameter("@RecurringTransactionId", TransactionID)                 );

                return results.FirstOrDefault();
            }
            catch (Exception ex)
            {
                // Handle/log exception here
                throw new Exception("Error fetching recurring transaction by ID", ex);
            }
        }


        public async Task<List<RecurringTransactionsEntity>> GetAllRecurringTransactionsAsync(int userId)
        {
            try
            {
                return await _helper.ExecuteStoredProcedureAsync(
                    "GetAllRecurringTransactions",
                    reader => new RecurringTransactionsEntity
                    {
                        RecurringTransactionId = reader.GetInt32(reader.GetOrdinal("RecurringTransactionId")),
                        UserID = reader.GetInt32(reader.GetOrdinal("UserId")),
                        CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                        Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                        FrequencyID = reader.GetInt32(reader.GetOrdinal("FrequencyID")),
                        StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                        EndDate = reader.IsDBNull(reader.GetOrdinal("EndDate")) ? null : reader.GetDateTime(reader.GetOrdinal("EndDate")),
                        NextRunDate = reader.GetDateTime(reader.GetOrdinal("NextRunDate")),
                        Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? string.Empty : reader.GetString(reader.GetOrdinal("Notes")),
                        IsAtive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                        IsOneTime = reader.GetBoolean(reader.GetOrdinal("IsOneTime"))
                    },
                    new SqlParameter("@UserId", userId)
                );
            }
            catch (Exception ex)
            {
                // Handle/log exception here
                throw new Exception("Error fetching all recurring transactions", ex);
            }
        }

        public async Task<RecurringTransaction> GetReccuringTransactionInfoByIdAsync(int transactionId, int userId)
        {
            try
            {
                if (string.IsNullOrEmpty(transactionId.ToString()))
                    throw new ValidationException("TransactionID does not exist");

                var Result = await _context.RecurringTransactions.FindAsync(transactionId);
                return Result;
            
            }
            catch (Exception ex)
            {
                // Handle/log exception here
                throw new Exception("Error fetching recurring transaction by ID", ex);
            }
        }

        public Task SaveChangesAsync()
        {
            // No implementation needed for direct database operations
            throw new NotImplementedException();
        }
    }
}
