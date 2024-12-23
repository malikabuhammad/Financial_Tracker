using FinancialTracker.Domain.Entites;
using FinancialTracker.Domain.Entities;
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
 
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<CategoryType> CategoryTypes { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<RecurringTransaction> RecurringTransactions { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<TransactionTag> TransactionTag { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
         public DbSet<Roles> Roles { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
        }


    }
}
