 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Domain.Entites;

public partial class Currency
{
    [Key]
    public int CurrencyId { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public double? ExchangeRate { get; set; }
}