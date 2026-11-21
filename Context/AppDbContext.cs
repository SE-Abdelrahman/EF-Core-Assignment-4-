using Assignment_4.Configurations;
using Assignment_4.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment_4.Context
{
    internal class AppDbContext : DbContext
    {
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAccount> CustomerAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=NationalBank;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountConfigs());
            modelBuilder.ApplyConfiguration(new BranchConfigs());
            modelBuilder.ApplyConfiguration(new CustomerAccountConfigs());
            modelBuilder.ApplyConfiguration(new CustomerConfigs());
            modelBuilder.ApplyConfiguration(new ManagerConfigs());
            modelBuilder.ApplyConfiguration(new TransactionConfigs());
        }
    }
}