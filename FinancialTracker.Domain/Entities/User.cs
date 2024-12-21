using FinancialTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinancialTracker.Domain.Entites;

public partial class Users
{
    [Key]
    public int UserId { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? UpdatedBy { get; set; }
    //public List<string> Roles { get; set; } = new List<string>();

    public virtual ICollection<Categories> Categories { get; set; } = new List<Categories>();
    public virtual ICollection<Roles> Roles { get; set; } = new List<Roles>();
}