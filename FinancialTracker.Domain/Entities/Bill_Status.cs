 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Domain.Entites;

public partial class Bill_Status
{
    [Key]
    public int? StatusID { get; set; }

    public string StatusName { get; set; }
}