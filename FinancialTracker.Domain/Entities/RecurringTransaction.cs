 
using System;
using System.ComponentModel.DataAnnotations;
namespace FinancialTracker.Domain.Entites;

public partial class RecurringTransaction
{
    [Key]
    public int RecurringTransactionId { get; set; }
    public int UserId { get; set; }
    public int CategoryId { get; set; }
    public decimal Amount { get; set; }
    public int FrequencyID { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Notes { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsOneTime { get; set; }
    public DateTime? CancelledDate { get; set; }
    public string? CancelReason { get; set; }
    public DateTime? NextRunDate { get; set; }
}