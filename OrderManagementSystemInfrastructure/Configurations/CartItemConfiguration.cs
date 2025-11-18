using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementSystemDomain.Models;

namespace OrderManagementSystemInfrastructure.Configurations
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            // Primary Key
            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.CartId)
                .IsRequired()
                .HasComment("Cart ID is required.");

            builder.HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Property(ci => ci.ProductId)
                .IsRequired()
                .HasComment("Product ID is required.");

            //builder.HasOne(ci => ci.Product)
            // .WithMany()
            // .HasForeignKey(ci => ci.ProductId)
            //.OnDelete(DeleteBehavior.Restrict)
            // .IsRequired();

            builder.Property(ci => ci.Quantity)
                .IsRequired()
                .HasComment("Quantity must be between 1 and 100.");

            builder.Property(ci => ci.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasComment("Unit Price must be between $0.01 and $10,000.00.");

            builder.Property(ci => ci.Discount)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m)
                .HasComment("Discount must be between $0.00 and $1,000.00.");

            builder.Property(ci => ci.TotalPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasComment("Total Price is calculated based on quantity, unit price, and discount.");

            builder.Property(ci => ci.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()")
                .HasComment("Timestamp when the cart item was created.");

            builder.Property(ci => ci.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()")
                .HasComment("Timestamp when the cart item was last updated.");

            // Indexes for performance
            builder.HasIndex(ci => ci.CartId);

            builder.HasIndex(ci => ci.ProductId);

            // Composite index for Cart and Product (useful for checking if product already in cart)
            builder.HasIndex(ci => new { ci.CartId, ci.ProductId });

            // Index for timestamp-based queries
            builder.HasIndex(ci => ci.CreatedAt);
        }
    }
}
