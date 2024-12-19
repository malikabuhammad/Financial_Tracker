using FinancialTracker.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTracker.Infrastructure
{
    public class AppDbContext:DbContext
    {

        public DbSet<Users> Users { get; set; }
        public DbSet<Bill> Bill { get; set; }
        public DbSet<Bill_Status> Bill_Status { get; set; }
        public DbSet<Budget> Budget { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<CategoryType> CategoryType { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<Goal> Goal { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<RecurringTransaction> RecurringTransaction { get; set; }
        public DbSet<Report> Report { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<TransactionTag> TransactionTag { get; set; }
        public DbSet<AuditLog> AuditLog { get; set; }
        public DbSet<Bill_Status> bill_Statuses { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
        }


    }
}
