using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTracker.Domain.Entities
{
    public class SpendingData
    {  
        public float Month { get; set; }

         public float CategoryId { get; set; }

         public float Income { get; set; }

         public float PreviousSpending { get; set; }
    }
}
