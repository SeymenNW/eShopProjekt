using Ardalis.GuardClauses;
using OrderService.Domain.Models;

public sealed class OrderItem
{
    private OrderItem() { } // EF

    public OrderItem(ItemSnapshot item, decimal unitPrice, int units, int orderId)  // Pass the orderId
    {
        Guard.Against.Null(item);
        Guard.Against.NegativeOrZero(unitPrice);
        Guard.Against.NegativeOrZero(units);

        Item = item;
        UnitPrice = unitPrice;
        Units = units;
        OrderId = orderId;  // Set the orderId
    }

    public int Id { get; private set; }
    public ItemSnapshot Item { get; private set; } = default!;
    public decimal UnitPrice { get; private set; }
    public int Units { get; private set; }

    // Foreign Key to Order
    public int OrderId { get; private set; }  // Correct property for the foreign key

    // Navigation property to Order
    public Order Order { get; private set; } = default!;  // Navigation property

    public decimal LineTotal => UnitPrice * Units;

    public void IncreaseUnits(int by)
    {
        Guard.Against.NegativeOrZero(by);
        Units += by;
    }

    public void UpdatePrice(decimal newPrice)
    {
        Guard.Against.NegativeOrZero(newPrice);
        UnitPrice = newPrice;
    }
}
