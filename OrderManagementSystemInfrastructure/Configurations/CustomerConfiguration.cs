using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementSystemDomain.Models;

namespace OrderManagementSystemInfrastructure.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            // Primary Key
            builder.HasKey(c => c.Id);

            builder.Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("First Name must be between 2 and 50 characters.");

            builder.Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("Last Name must be between 2 and 50 characters.");

            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(256)
                .HasComment("Email is required.");

            builder.Property(c => c.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("PhoneNumber is required.");

            builder.Property(c => c.DateOfBirth)
                .IsRequired()
                .HasComment("DateOfBirth is required.");

            builder.Property(c => c.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Unique Index on Email
            builder.HasIndex(c => c.Email)
                .IsUnique()
                .HasDatabaseName("IX_Email_Unique");



            // Index on PhoneNumber for lookups
            builder.HasIndex(c => c.PhoneNumber);

            // Index on IsActive for filtering active customers
            builder.HasIndex(c => c.IsActive);

            // Composite index for name searches
            builder.HasIndex(c => new { c.FirstName, c.LastName });
        }
    }
}
