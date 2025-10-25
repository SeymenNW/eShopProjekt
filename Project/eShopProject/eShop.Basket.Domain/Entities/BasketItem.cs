namespace eShop.Basket.Domain.Entities;

public class BasketItem
{
    public int Id { get; set; }                     // Primærnøgle (EF Core)
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string PictureUrl { get; set; } = string.Empty;   // ← NY: billede-link
    public Guid ShoppingBasketId { get; set; }

    public BasketItem(string productId, string productName, decimal price, int quantity, string pictureUrl = "")
    {
        ProductId = productId;
        ProductName = productName;
        Price = price;
        Quantity = quantity;
        PictureUrl = pictureUrl;
    }

    // EF Core kræver en parameterløs constructor
    public BasketItem() { }
}
