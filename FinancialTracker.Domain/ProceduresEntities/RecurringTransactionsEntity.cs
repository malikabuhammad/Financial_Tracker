using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTracker.Domain.ProceduresEntities
{
    public class RecurringTransactionsEntity
    {
        public int RecurringTransactionId { get; set; }
        public int? UserID { get; set; } 
        public int? CategoryID { get; set; } 
        public string CategoryName { get; set; } 
        public decimal? Amount { get; set; } 
        public int? FrequencyID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? NextRunDate { get; set; }
        public string Notes { get; set; }
        public bool? IsAtive { get; set; }   
        public bool? IsOneTime { get; set; }
    }
}
