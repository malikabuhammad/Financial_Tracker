 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Domain.Entites;

public partial class Categories
{
    [Key]
    public int CategoryId { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; }

    public int TypeId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Users User { get; set; }
}