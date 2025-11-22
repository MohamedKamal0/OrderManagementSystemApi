using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementSystemDomain.Models;

namespace OrderManagementSystemInfrastructure.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            // Primary Key
            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.OrderId)
                .IsRequired()
                .HasComment("Order ID is required.");

            builder.HasOne(oi => oi.Order)
                         .WithMany(o => o.OrderItems)
                         .HasForeignKey(oi => oi.OrderId)
                         .OnDelete(DeleteBehavior.Cascade)  // or Restrict, based on your needs
                         .IsRequired();

            builder.Property(oi => oi.ProductId)
                .IsRequired()
                .HasComment("Product ID is required.");

            
            builder.Property(oi => oi.Quantity)
                .IsRequired()
                .HasComment("Quantity must be between 1 and 100.");

            builder.Property(oi => oi.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasComment("Unit Price must be between $0.00 and $10,000.00.");

            builder.Property(oi => oi.Discount)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m)
                .HasComment("Discount must be between $0.00 and $10,000.00.");

            builder.Property(oi => oi.TotalPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasComment("Total Price must be between $0.00 and $10,000.00.");

            builder.HasIndex(oi => oi.OrderId);

            builder.HasIndex(oi => oi.ProductId);

            // Composite index for Order and Product
            builder.HasIndex(oi => new { oi.OrderId, oi.ProductId });
        }
    }
}
