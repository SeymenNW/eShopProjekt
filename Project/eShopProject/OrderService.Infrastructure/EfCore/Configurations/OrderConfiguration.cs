using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Models;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> b)
    {
        b.ToTable("OrderItems");
        b.HasKey(x => x.Id);

        // Configuring ItemSnapshot as owned by OrderItem
        b.OwnsOne(x => x.Item, i =>
        {
            i.Property(p => p.ItemId).IsRequired();
            i.Property(p => p.Name).HasMaxLength(200).IsRequired();
            i.Property(p => p.PictureUri).HasMaxLength(1024);
            i.ToTable("OrderItemSnapshots");  // Table for ItemSnapshot
        });

        b.Property(x => x.UnitPrice).HasPrecision(18, 2);  // Precision for UnitPrice
        b.Property(x => x.Units).IsRequired();  // Ensure Units is required
    }
}


