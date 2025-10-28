using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystemDomain.Models;

namespace OrderManagementSystemInfrastructure.Data
{
    public class AppDbContext : DbContext
    {
        // Constructor accepting DbContextOptions
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        // Configuring model properties and relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Order -> BillingAddress 
            modelBuilder.Entity<Order>()
                .HasOne(o => o.BillingAddress)
                .WithMany()
                .HasForeignKey(o => o.BillingAddressId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete
            // Order -> ShippingAddress 
            modelBuilder.Entity<Order>()
                .HasOne(o => o.ShippingAddress)
                .WithMany()
                .HasForeignKey(o => o.ShippingAddressId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete
            // Order -> Cancellation 
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Cancellation)
                .WithOne(c => c.Order)
                .HasForeignKey<Cancellation>(c => c.OrderId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete to avoid multiple paths
            // Payment -> Refund 
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Refund)
                .WithOne(r => r.Payment)
                .HasForeignKey<Refund>(r => r.PaymentId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete to avoid multiple paths
            // Feedback -> Customer (Many-to-One)
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Customer)
                .WithMany(c => c.Feedbacks)
                .HasForeignKey(f => f.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
            // Feedback -> Product (Many-to-One)
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Product)
                .WithMany(p => p.Feedbacks)
                .HasForeignKey(f => f.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            // Initial Seed Data
            // Seed OrderStatusEntity with initial data
            
        }
        // DbSets representing tables in the database
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Cancellation> Cancellations { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
    }
}
