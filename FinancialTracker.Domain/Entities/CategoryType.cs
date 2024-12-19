 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Domain.Entites;

public partial class CategoryType
{
    [Key]
    public int TypeId { get; set; }

    public string TypeName { get; set; }
}