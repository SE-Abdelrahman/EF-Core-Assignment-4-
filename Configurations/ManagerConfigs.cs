using Assignment_4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Assignment_4.Configurations
{
    internal class ManagerConfigs : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.FullName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(m => m.Email)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(m => m.PhoneNumber)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(m => m.HireDate)
                   .HasColumnType("date");

            builder.HasData(
                new Manager { Id = 1, FullName = "Mahmoud Hassan", Email = "mahmoud@nbank.com", PhoneNumber = "01001112223", HireDate = new DateTime(2020, 1, 15) },
                new Manager { Id = 2, FullName = "Nadia Kamal", Email = "nadia@nbank.com", PhoneNumber = "01004445556", HireDate = new DateTime(2021, 5, 10) }
            );
        }
    }
}