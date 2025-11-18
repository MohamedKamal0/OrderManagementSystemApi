using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementSystemDomain.Models;

namespace OrderManagementSystemInfrastructure.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // Primary Key
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("Category Name must be between 3 and 100 characters.");

            builder.Property(c => c.Description)
                .HasMaxLength(500)
                .HasComment("Description cannot exceed 500 characters.");

            builder.Property(c => c.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Products Relationship (One-to-Many)
            //   builder.HasMany(c => c.Products)
            //.WithOne(p => p.Category)
            // .HasForeignKey(p => p.CategoryId)
            // .OnDelete(DeleteBehavior.Restrict);

            // Unique Index on Name
            builder.HasIndex(c => c.Name)
                .IsUnique()
                .HasDatabaseName("IX_Name_Unique");

            // Index on IsActive for filtering active categories
            builder.HasIndex(c => c.IsActive);
        }
    }
}
