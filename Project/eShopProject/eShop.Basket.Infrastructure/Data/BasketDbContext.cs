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
            .ToTable("Baskets")
            .HasMany(b => b.Items)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BasketItem>()
            .ToTable("BasketItems")
            .HasKey(i => i.Id);

        base.OnModelCreating(modelBuilder);
    }

}
