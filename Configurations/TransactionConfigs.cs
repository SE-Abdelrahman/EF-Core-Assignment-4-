using Assignment_4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assignment_4.Configurations
{
    internal class TransactionConfigs : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.TransactionNumber);

            builder.Property(t => t.TransactionNumber)
                   .HasMaxLength(30);

            builder.Property(t => t.Amount)
                   .HasColumnType("decimal")
                   .HasPrecision(18, 2);

            builder.Property(t => t.Note)
                   .HasMaxLength(300);

            builder.HasOne(t => t.Account)
                   .WithMany(a => a.Transactions)
                   .HasForeignKey(t => t.AccountNumber)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}