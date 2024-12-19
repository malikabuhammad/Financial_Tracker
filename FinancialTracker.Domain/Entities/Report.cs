 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Domain.Entites;

public partial class Report
{
    [Key]
    public int ReportId { get; set; }

    public int? UserId { get; set; }

    public string ReportName { get; set; }

    public DateTime? GeneratedAt { get; set; }

    public string FilePath { get; set; }
}