using eShop.Basket.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Basket.Infrastructure.Data;

public class BasketDbContext : DbContext
{
    public BasketDbContext(DbContextOptions<BasketDbContext> options) : base(options) { }

    // DbSets
    public DbSet<ShoppingBasket> Baskets => Set<ShoppingBasket>();
    public DbSet<BasketItem> BasketItems => Set<BasketItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShoppingBasket>()
            .ToTable("baskets")
            .HasKey(b => b.Id);

        modelBuilder.Entity<BasketItem>()
            .ToTable("basketitems")
            .HasKey(i => i.Id);

        modelBuilder.Entity<ShoppingBasket>()
            .HasMany(b => b.Items)
            .WithOne()                                  // ingen navigation tilbage
            .HasForeignKey(i => i.ShoppingBasketId)     // ← eksplicit FK
            .OnDelete(DeleteBehavior.Cascade);
    }


}
