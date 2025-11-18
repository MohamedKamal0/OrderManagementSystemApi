using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementSystemDomain.Models;

namespace OrderManagementSystemInfrastructure.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            // Primary Key
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CustomerId)
                .IsRequired()
                .HasComment("Customer ID is required.");

            builder.HasOne(c => c.Customer)
                .WithMany(cu => cu.Carts)
                .HasForeignKey(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Property(c => c.IsCheckedOut)
                .IsRequired()
                .HasDefaultValue(false)
                .HasComment("Indicates whether the cart has been checked out.");

            builder.Property(c => c.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()")
                .HasComment("Timestamp when the cart was created.");

            builder.Property(c => c.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()")
                .HasComment("Timestamp when the cart was last updated.");

            // CartItems Relationship (One-to-Many)
            //builder.HasMany(c => c.CartItems)
            //.WithOne(ci => ci.Cart)
            //  .HasForeignKey(ci => ci.CartId)
            // .OnDelete(DeleteBehavior.Cascade);

            // Indexes for performance
            builder.HasIndex(c => c.CustomerId);

            builder.HasIndex(c => c.IsCheckedOut);

            // Composite index for finding active cart per customer
            builder.HasIndex(c => new { c.CustomerId, c.IsCheckedOut });

            // Index for timestamp-based queries
            builder.HasIndex(c => c.CreatedAt);

            builder.HasIndex(c => c.UpdatedAt);
        }
    }
}
