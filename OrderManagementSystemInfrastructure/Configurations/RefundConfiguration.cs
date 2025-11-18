using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementSystemDomain.Models;

namespace OrderManagementSystemInfrastructure.Configurations
{
    public class RefundConfiguration : IEntityTypeConfiguration<Refund>
    {
        public void Configure(EntityTypeBuilder<Refund> builder)
        {
            // Primary Key
            builder.HasKey(r => r.Id);

            builder.Property(r => r.CancellationId)
                .IsRequired()
                .HasComment("Cancellation ID is required.");

            //   builder.HasOne(r => r.Cancellation)
            //.WithOne(c => c.Refund)
            //.HasForeignKey<Refund>(r => r.CancellationId)
            //.OnDelete(DeleteBehavior.Cascade)
            // .IsRequired();

            builder.Property(r => r.PaymentId)
                .IsRequired()
                .HasComment("Payment ID is required.");

            builder.HasOne(r => r.Payment)
                .WithOne(p => p.Refund)
                .HasForeignKey<Refund>(r => r.PaymentId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Property(r => r.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasComment("Refund amount must be between $0.01 and $100,000.00.");

            builder.Property(r => r.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasComment("Status of the refund.");

            builder.Property(r => r.RefundMethod)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("Method used for refund (e.g., Original Payment Method, Bank Transfer).");

            builder.Property(r => r.RefundReason)
                .HasMaxLength(500)
                .HasComment("Refund Reason cannot exceed 500 characters.");

            builder.Property(r => r.TransactionId)
                .HasMaxLength(100)
                .HasComment("Transaction ID cannot exceed 100 characters.");

            builder.Property(r => r.InitiatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()")
                .HasComment("Date and time when the refund was initiated.");

            // Completed At
            builder.Property(r => r.CompletedAt)
                .HasComment("Date and time when the refund was completed.");

            // Processed By
            builder.Property(r => r.ProcessedBy)
                .HasComment("Track who processed (approved) the refund.");

            // Indexes for performance
            builder.HasIndex(r => r.CancellationId)
                .IsUnique();

            builder.HasIndex(r => r.PaymentId)
                .IsUnique();

            builder.HasIndex(r => r.Status);

            builder.HasIndex(r => r.TransactionId);

            builder.HasIndex(r => r.InitiatedAt);

            builder.HasIndex(r => r.CompletedAt);

            // Composite index for filtering by status and initiated date
            builder.HasIndex(r => new { r.Status, r.InitiatedAt });

            // Composite index for processing queries
            builder.HasIndex(r => new { r.Status, r.ProcessedBy });
        }
    }
}
