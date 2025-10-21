using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Models;

namespace OrderService.Infrastructure.EfCore
{
    public sealed class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring Order to own Address
            modelBuilder.Entity<Order>()
                .OwnsOne(o => o.ShipToAddress, a =>
                {
                    a.Property(p => p.Street).HasMaxLength(200);
                    a.Property(p => p.City).HasMaxLength(100);
                    a.Property(p => p.Zip).HasMaxLength(20);
                    a.Property(p => p.Country).HasMaxLength(90);
                    a.ToTable("OrderAddresses");
                });

            // Configuring ItemSnapshot as owned by OrderItem
            modelBuilder.Entity<OrderItem>()
                .OwnsOne(x => x.Item, i =>
                {
                    i.Property(p => p.ItemId).IsRequired();  // Define ItemId for ItemSnapshot
                    i.Property(p => p.Name).HasMaxLength(200).IsRequired();  // Define Name for ItemSnapshot
                    i.Property(p => p.PictureUri).HasMaxLength(1024);  // Define PictureUri for ItemSnapshot
                    i.ToTable("OrderItemSnapshots");  // The table where ItemSnapshot will be stored
                });

            // Do NOT configure HasNoKey() here for ItemSnapshot

            // Configure relationship between Order and OrderItem
            modelBuilder.Entity<OrderItem>()
                .HasOne(o => o.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(o => o.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
