using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTracker.Application.DTOs
{
    public class RecurringTransactionDto
    {
        public class RecurringTransactionEditModel
        {
            public int RecurringTransactionId { get; set; }
            public int CategoryId { get; set; }
            public decimal Amount { get; set; }
            public int FrequencyID { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public DateTime? CancelledDate { get; set; }
            public string? CancelReason { get; set; }
            public DateTime? NextRunDate { get; set; }
            public string Notes { get; set; }
            public bool IsOneTime { get; set; }
            public bool IsActive { get; set; }
        }

        
            public class RecurringTransactionAddModel
        {
                 
                public int CategoryId { get; set; }
                public decimal Amount { get; set; }
                public int FrequencyID { get; set; }
                public DateTime StartDate { get; set; }
                public DateTime? EndDate { get; set; }
                public string Notes { get; set; }
                public bool IsOneTime { get; set; }
            }
        }
    }
 
