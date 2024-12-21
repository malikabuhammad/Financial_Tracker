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
    }
}
