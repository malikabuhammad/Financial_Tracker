 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Domain.Entites;

public partial class Goal
{
    [Key]
    public int GoalId { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; }

    public decimal TargetAmount { get; set; }

    public decimal? SavedAmount { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? CreatedAt { get; set; }
}