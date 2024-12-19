 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Domain.Entites;

public partial class Notification
{
    [Key]
    public int NotificationId { get; set; }

    public int UserId { get; set; }

    public string Message { get; set; }

    public bool? IsRead { get; set; }

    public DateTime? CreatedAt { get; set; }
}