using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Domain.Entites;

public partial class Tag
{
    [Key]
    public int TagId { get; set; }

    public string Name { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<TransactionTag> TransactionTags { get; set; } = new List<TransactionTag>();
}