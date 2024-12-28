using System.Data;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using FinancialTracker.Infrastructure.Utilities;

namespace FinancialTracker.Infrastructure.Repositories
{
    public class SpendingDataExportService
    {
        private readonly ProceduresHelper _helper;

        public SpendingDataExportService(ProceduresHelper helper)
        {
            _helper = helper;
        }

        // Method to export spending data to a CSV file
        public async Task ExportSpendingDataToCsvAsync(string filePath)
        {
            // Use ProceduresHelper to execute the stored procedure and fetch data
            var dataTable = await _helper.ExecuteStoredProcedureAsync("GetMonthlySpendingData");

            // Export the DataTable to CSV
            await WriteToCsvAsync(filePath, dataTable);
        }

        private async Task WriteToCsvAsync(string filePath, DataTable dataTable)
        {
            // Open the file for writing
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                // Write column headers
                foreach (DataColumn column in dataTable.Columns)
                {
                    csv.WriteField(column.ColumnName);
                }
                await csv.NextRecordAsync();

                // Write rows
                foreach (DataRow row in dataTable.Rows)
                {
                    foreach (var item in row.ItemArray)
                    {
                        csv.WriteField(item);
                    }
                    await csv.NextRecordAsync();
                }
            }
        }
    }
}
