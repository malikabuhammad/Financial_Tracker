using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Domain.Entites;

public partial class TransactionTag
{
    [Key]
    public int TransactionTagId { get; set; }

    public int TransactionId { get; set; }

    public int TagId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Tag Tag { get; set; }

    public virtual Transactions Transaction { get; set; }
}