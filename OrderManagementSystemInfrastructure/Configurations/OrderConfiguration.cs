using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementSystemDomain.Models;

namespace OrderManagementSystemInfrastructure.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Primary Key
            builder.HasKey(o => o.Id);

            builder.Property(o => o.OrderNumber)
      .IsRequired()
      .HasMaxLength(30)
      .HasComment("Order Number cannot exceed 30 characters.");

            builder.Property(o => o.OrderDate)
                .IsRequired()
                .HasComment("Order Date is required.");

            builder.Property(o => o.CustomerId)
                .IsRequired()
                .HasComment("Customer ID is required.");

            builder.HasOne(o => o.Customer)
                 .WithMany(c => c.Orders)  
                 .HasForeignKey(o => o.CustomerId)
                 .OnDelete(DeleteBehavior.Restrict)
                 .IsRequired();

            builder.Property(o => o.BillingAddressId)
                .IsRequired()
                .HasComment("Billing Address ID is required.");

            builder.HasOne(o => o.BillingAddress)
                .WithMany()
                .HasForeignKey(o => o.BillingAddressId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.Property(o => o.ShippingAddressId)
                .IsRequired()
                .HasComment("Shipping Address ID is required.");

            builder.HasOne(o => o.ShippingAddress)
                .WithMany()
                .HasForeignKey(o => o.ShippingAddressId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.Property(o => o.TotalBaseAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasComment("Total Base Amount must be between $0.00 and $100,000.00.");

            builder.Property(o => o.TotalDiscountAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasComment("Total Discount Amount must be between $0.00 and $100,000.00.");

            builder.Property(o => o.ShippingCost)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m)
                .HasComment("Shipping Cost must be between $0.00 and $10,000.00.");

            builder.Property(o => o.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasComment("Total Amount must be between $0.00 and $110,000.00.");

            builder.Property(o => o.OrderStatus)
                .IsRequired()
                .HasConversion<string>()
                .HasComment("Invalid Order Status.");

            
            // Payment Relationship (One-to-One)
            builder.HasOne(o => o.Payment)
                     .WithOne(p => p.Order)  
                     .HasForeignKey<Payment>(p => p.OrderId)  
                     .OnDelete(DeleteBehavior.Restrict);
            
            // Index for Order Number (for faster lookups)
            builder.HasIndex(o => o.OrderNumber)
                .IsUnique();

            // Index for CustomerId
            builder.HasIndex(o => o.CustomerId);

            // Index for OrderDate
            builder.HasIndex(o => o.OrderDate);
        }
    }
}
