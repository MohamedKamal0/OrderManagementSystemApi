using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementSystemDomain.Models;

namespace OrderManagementSystemInfrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Primary Key
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("Product Name must be between 3 and 100 characters.");

            builder.Property(p => p.Description)
                .IsRequired()
                .HasComment("Description must be at least 10 characters.");

            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasComment("Price must be between $0.01 and $10,000.00.");

            builder.Property(p => p.StockQuantity)
                .IsRequired()
                .HasDefaultValue(0)
                .HasComment("Stock Quantity must be between 0 and 1000.");

            builder.Property(p => p.ImageUrl)
                .HasMaxLength(500);

            builder.Property(p => p.DiscountPercentage)
                .IsRequired()
                .HasDefaultValue(0)
                .HasComment("Discount Percentage must be between 0% and 100%.");

            builder.Property(p => p.IsAvailable)
              //  .IsRequired()
              .HasDefaultValue(true);

            builder.Property(p => p.CategoryId)
                .IsRequired()
                .HasComment("Category ID is required.");

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // OrderItems Relationship (One-to-Many)
            builder.HasMany(p => p.OrderItems)
                .WithOne(oi => oi.Product)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Feedbacks Relationship (One-to-Many)
            //  builder.HasMany(p => p.Feedbacks)
            //.WithOne(f => f.Product)
            //.HasForeignKey(f => f.ProductId)
            // .OnDelete(DeleteBehavior.Cascade);

            // Indexes for performance
            builder.HasIndex(p => p.Name);

            builder.HasIndex(p => p.CategoryId);

            builder.HasIndex(p => p.IsAvailable);

            builder.HasIndex(p => p.Price);

            // Composite index for filtering available products by category
            builder.HasIndex(p => new { p.CategoryId, p.IsAvailable });

            // Composite index for price range queries with availability
            builder.HasIndex(p => new { p.IsAvailable, p.Price });
        }
    }
}
