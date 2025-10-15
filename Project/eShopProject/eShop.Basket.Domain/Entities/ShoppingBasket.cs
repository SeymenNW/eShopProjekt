namespace eShop.Basket.Domain.Entities;

public class ShoppingBasket
{
    public Guid Id { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public List<BasketItem> Items { get; set; } = new();
    public decimal TotalPrice => Items.Sum(i => i.Quantity * i.Price);

    public void AddItem(string productId, string productName, decimal price, int quantity = 1)
    {
        var existing = Items.FirstOrDefault(i => i.ProductId == productId);
        if (existing != null)
            existing.Quantity += quantity;
        else
            Items.Add(new BasketItem(productId, productName, price, quantity));
    }

    public void RemoveItem(string productId)
    {
        Items.RemoveAll(i => i.ProductId == productId);
    }

    public void Clear() => Items.Clear();
}
