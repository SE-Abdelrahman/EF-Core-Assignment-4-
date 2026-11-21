using Assignment_4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assignment_4.Configurations
{
    internal class BranchConfigs : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.HasKey(b => b.Code);

            builder.Property(b => b.Code)
                   .HasMaxLength(20);

            builder.Property(b => b.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(b => b.Address)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(b => b.PhoneNumber)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.HasOne(b => b.Manager)
                   .WithOne(m => m.Branch)
                   .HasForeignKey<Branch>(b => b.ManagerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new Branch { Code = "CAI-01", Name = "Cairo Main Branch", Address = "Downtown, Cairo", PhoneNumber = "0225551111", ManagerId = 1 },
                new Branch { Code = "ALX-01", Name = "Alexandria Branch", Address = "Smouha, Alexandria", PhoneNumber = "034442222", ManagerId = 2 }
            );
        }
    }
}