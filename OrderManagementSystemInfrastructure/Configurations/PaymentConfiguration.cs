using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementSystemDomain.Models;

namespace OrderManagementSystemInfrastructure.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            // Primary Key
            builder.HasKey(p => p.Id);

            builder.Property(p => p.OrderId)
                .IsRequired()
                .HasComment("Order ID is required.");

            //    builder.HasOne(p => p.Order)
            //.WithOne(o => o.Payment)
            //.HasForeignKey<Payment>(p => p.OrderId)
            //.OnDelete(DeleteBehavior.Restrict)
            //.IsRequired();

            builder.Property(p => p.PaymentMethod)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("Payment method (e.g., DebitCard, CreditCard, PayPal, COD).");

            builder.Property(p => p.TransactionId)
                .HasMaxLength(50)
                .HasComment("Transaction ID from payment gateway.");

            builder.Property(p => p.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasComment("Payment amount.");

            builder.Property(p => p.PaymentDate)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()")
                .HasComment("Date and time when the payment was made.");


            builder.Property(p => p.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasComment("Payment status (Completed, Pending, Failed, Refunded).");

            // Refund Relationship (One-to-One)
            //  builder.HasOne(p => p.Refund)
            //.WithOne(r => r.Payment)
            //.HasForeignKey<Refund>(r => r.PaymentId)
            // .OnDelete(DeleteBehavior.Cascade);

            // Indexes for performance
            builder.HasIndex(p => p.OrderId)
                .IsUnique();

            builder.HasIndex(p => p.TransactionId);

            builder.HasIndex(p => p.Status);

            builder.HasIndex(p => p.PaymentDate);

            builder.HasIndex(p => p.PaymentMethod);

            // Composite index for filtering by status and date
            builder.HasIndex(p => new { p.Status, p.PaymentDate });

            // Composite index for payment method and status
            builder.HasIndex(p => new { p.PaymentMethod, p.Status });
        }
    }
}
