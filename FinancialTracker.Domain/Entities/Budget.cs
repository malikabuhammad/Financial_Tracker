 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Domain.Entites;

public partial class Budget
{
    [Key]
    public int BudgetId { get; set; }

    public int UserId { get; set; }

    public int CategoryId { get; set; }

    public decimal Amount { get; set; }

    public int PeriodId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}