using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace FinancialTracker.Infrastructure.Utilities
{
    public class ProceduresHelper
    {
        private readonly IConfiguration _configuration;

        public ProceduresHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<T>> ExecuteStoredProcedureAsync<T>(string storedProcedureName, Func<SqlDataReader, T> mapFunction, params SqlParameter[] parameters)
        {
            var resultList = new List<T>();
            var connectionString = _configuration.GetConnectionString("MyConnection");

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var item = mapFunction(reader);
                                resultList.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Add logging or rethrow as needed
                throw new Exception($"An error occurred while executing stored procedure {storedProcedureName}: {ex.Message}", ex);
            }

            return resultList;
        }

        public async Task<T> ExecuteScalarStoredProcedureAsync<T>(string storedProcedureName, params SqlParameter[] parameters)
        {
            var connectionString = _configuration.GetConnectionString("MyConnection");

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        // Execute the stored procedure and return the scalar value
                        var result = await command.ExecuteScalarAsync();
                        return (result == DBNull.Value || result == null) ? default : (T)result;
                    }
                }
            }
            catch (Exception ex)
            {
                // Add logging or rethrow as needed
                throw new Exception($"An error occurred while executing stored procedure {storedProcedureName}: {ex.Message}", ex);
            }
        }




        //returns the row affected , used when no needing to return data 
        public async Task<int> ExecuteNonQueryAsync(string storedProcedureName, params SqlParameter[] parameters)
        {
            var connectionString = _configuration.GetConnectionString("MyConnection");

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        // Execute the stored procedure as a non-query and return the affected row count
                        return await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while executing stored procedure {storedProcedureName}: {ex.Message}", ex);
            }
        }


        


        // for when you have an output parameter 

        public async Task<T> ExecuteStoredProcedureWithOutputAsync<T>(string storedProcedureName, SqlParameter outputParameter, params SqlParameter[] inputParameters)
        {
            var connectionString = _configuration.GetConnectionString("MyConnection");

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        if (inputParameters != null)
                        {
                            command.Parameters.AddRange(inputParameters);
                        }

                        // Add the output parameter
                        command.Parameters.Add(outputParameter);

                        await command.ExecuteNonQueryAsync();

                        // Retrieve the value of the output parameter
                        return (T)command.Parameters[outputParameter.ParameterName].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while executing stored procedure {storedProcedureName}: {ex.Message}", ex);
            }
        }




        //for transactions 
        public async Task ExecuteStoredProcedureWithTransactionAsync(IEnumerable<(string ProcedureName, SqlParameter[] Parameters)> procedures)
        {
            var connectionString = _configuration.GetConnectionString("MyConnection");

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            foreach (var (procedureName, parameters) in procedures)
                            {
                                using (var command = new SqlCommand(procedureName, connection, transaction))
                                {
                                    command.CommandType = CommandType.StoredProcedure;

                                    if (parameters != null)
                                    {
                                        command.Parameters.AddRange(parameters);
                                    }

                                    await command.ExecuteNonQueryAsync();
                                }
                            }

                            // Commit the transaction if all procedures succeed
                            transaction.Commit();
                        }
                        catch
                        {
                            // Rollback the transaction if any procedure fails
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while executing stored procedures with transaction: {ex.Message}", ex);
            }
        }

    }
}
