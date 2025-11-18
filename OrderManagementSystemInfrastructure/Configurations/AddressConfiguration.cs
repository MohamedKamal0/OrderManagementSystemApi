using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementSystemDomain.Models;

namespace OrderManagementSystemInfrastructure.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            // Primary Key
            builder.HasKey(a => a.Id);

            builder.Property(a => a.CustomerId)
                .IsRequired()
                .HasComment("Customer ID is required.");

            builder.HasOne(a => a.Customer)
                .WithMany(c => c.Addresses)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Property(a => a.AddressLine)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("Address Line 1 cannot exceed 100 characters.");

            builder.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("City cannot exceed 50 characters.");

            builder.Property(a => a.State)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("State cannot exceed 50 characters.");

            builder.Property(a => a.PostalCode)
                .IsRequired()
                .HasMaxLength(20)
                .HasComment("Postal Code is required.");

            // Indexes for performance
            builder.HasIndex(a => a.CustomerId);

            // Composite index for location-based searches
            builder.HasIndex(a => new { a.City, a.State });

            // Index for postal code lookups
            builder.HasIndex(a => a.PostalCode);
        }
    }
}
