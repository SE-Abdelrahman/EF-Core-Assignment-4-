using Assignment_4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assignment_4.Configurations
{
    internal class CustomerConfigs : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.FullName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.NationalId)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(c => c.Email)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(c => c.PhoneNumber)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(c => c.Address)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(c => c.DateOfBirth)
                   .HasColumnType("date");
        }
    }
}