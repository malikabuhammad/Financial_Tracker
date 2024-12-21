using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTracker.Application.DTOs
{
    public class TransactionsDTOs
    {
        public class TransactionAddModel
        {
 
 
            public int CategoryId { get; set; }

            public decimal Amount { get; set; }

            public DateTime Date { get; set; }

            public string Notes { get; set; }

            public bool? IsRecurring { get; set; }

 

        }
        public class TransactionEditModel
        {
            public int TransactionId { get; set; }


            public int CategoryId { get; set; }

            public decimal Amount { get; set; }

            public DateTime Date { get; set; }

            public string Notes { get; set; }

            public bool? IsRecurring { get; set; }


        }
    }
}
