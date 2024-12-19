using System;
using System.Collections.Generic;

namespace FinancialTracker.Domain.Entites;

public  class Transactions
{
    public int TransactionId { get; set; }

    public int UserId { get; set; }

    public int CategoryId { get; set; }

    public decimal Amount { get; set; }

    public DateTime Date { get; set; }

    public string Notes { get; set; }

    public bool? IsRecurring { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual ICollection<TransactionTag> TransactionTags { get; set; } = new List<TransactionTag>();
}