namespace eShop.Basket.Domain.Entities;

public class BasketItem
{
    public int Id { get; set; }  // ← Primærnøgle kræves af EF Core
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public BasketItem(string productId, string productName, decimal price, int quantity)
    {
        ProductId = productId;
        ProductName = productName;
        Price = price;
        Quantity = quantity;
    }

    // EF Core kræver en parameterløs constructor
    public BasketItem() { }
}
