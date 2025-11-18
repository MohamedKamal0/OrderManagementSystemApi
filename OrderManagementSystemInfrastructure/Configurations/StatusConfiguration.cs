using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagementSystemDomain.Models;

namespace OrderManagementSystemInfrastructure.Configurations
{
    public class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            // Primary Key
            builder.HasKey(s => s.Id);


            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("Status name is required.");

            // Unique Index on Name
            builder.HasIndex(s => s.Name)
                .IsUnique()
                .HasDatabaseName("IX_Status_Name_Unique");
        }
    }
}
