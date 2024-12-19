 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Domain.Entites;

public partial class AuditLog
{
    [Key]
    public int LogId { get; set; }

    public int UserId { get; set; }

    public string Action { get; set; }

    public string Entity { get; set; }

    public int? EntityId { get; set; }

    public DateTime? Timestamp { get; set; }

    public string Details { get; set; }
}