using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementSystemDomain.Models;

namespace OrderManagementSystemInfrastructure.Configurations
{
    public class CancellationConfiguration : IEntityTypeConfiguration<Cancellation>
    {
        public void Configure(EntityTypeBuilder<Cancellation> builder)
        {
            // Primary Key
            builder.HasKey(c => c.Id);

            builder.Property(c => c.OrderId)
                .IsRequired()
                .HasComment("Order ID is required.");

            builder.HasOne(c => c.Order)
           .WithOne(o => o.Cancellation)
          .HasForeignKey<Cancellation>(c => c.OrderId)
           .OnDelete(DeleteBehavior.Restrict)
             .IsRequired();

            builder.Property(c => c.Reason)
                .IsRequired()
                .HasMaxLength(500)
                .HasComment("Cancellation reason cannot exceed 500 characters.");

            builder.Property(c => c.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasComment("Status of the cancellation request.");

            builder.Property(c => c.RequestedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()")
                .HasComment("Date and time when the cancellation was requested.");

            builder.Property(c => c.ProcessedAt)
                .HasComment("Date and time when the cancellation was processed.");

            builder.Property(c => c.ProcessedBy)
                .HasComment("ID of the admin or system that processed the cancellation.");

            builder.Property(c => c.OrderAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasComment("The order amount at the time of cancellation request initiation.");

            builder.Property(c => c.CancellationCharges)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0.00m)
                .HasComment("The cancellation charges applied (if any).");

            builder.Property(c => c.Remarks)
                .HasMaxLength(500)
                .HasComment("Remarks cannot exceed 500 characters.");

            // Refund Relationship (One-to-One)
            builder.HasOne(c => c.Refund)
                .WithOne(r => r.Cancellation)
                .HasForeignKey<Refund>(r => r.CancellationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes for performance
            builder.HasIndex(c => c.OrderId)
                .IsUnique();

            builder.HasIndex(c => c.Status);

            builder.HasIndex(c => c.RequestedAt);

            builder.HasIndex(c => c.ProcessedAt);

            // Composite index for filtering by status and requested date
            builder.HasIndex(c => new { c.Status, c.RequestedAt });
        }
    }
}
