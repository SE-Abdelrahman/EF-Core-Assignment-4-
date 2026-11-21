using Assignment_4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assignment_4.Configurations
{
    internal class AccountConfigs : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(a => a.AccountNumber);

            builder.Property(a => a.AccountNumber)
                   .HasMaxLength(30);

            builder.Property(a => a.CurrentBalance)
                   .HasColumnType("decimal")
                   .HasPrecision(18, 2);

            builder.HasOne(a => a.Branch)
                   .WithMany(b => b.Accounts)
                   .HasForeignKey(a => a.BranchCode)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}