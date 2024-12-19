using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTracker.Domain.ProceduresEntities
{
    public class TransactionsEntity
    {
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreateDate { get; set; }
        public string Notes { get; set; }
        public bool IsRecurring { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string CategoryName { get; set; }
        public string TypeName { get; set; }
        public int CategoryId { get; set; }
        public int TypeId { get; set; }
    }
}
