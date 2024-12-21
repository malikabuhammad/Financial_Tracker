using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTracker.Domain.ProceduresEntities
{
    public class CategoriesEntity
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
    }
}
