﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Domain.Entites;

public partial class Bill
{
    [Key]
    public int BillId { get; set; }

    public int UserId { get; set; }

    public string BillName { get; set; }

    public decimal Amount { get; set; }

    public DateOnly DueDate { get; set; }

    public bool IsRecurring { get; set; }

    public string Frequency { get; set; }

    public int? CategoryId { get; set; }

    public int? StatusID { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}