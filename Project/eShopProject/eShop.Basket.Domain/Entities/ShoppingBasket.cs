namespace eShop.Basket.Domain.Entities;

public class ShoppingBasket
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string CustomerId { get; set; } = string.Empty;
    public List<BasketItem> Items { get; set; } = new();
    public decimal TotalPrice => Items.Sum(i => i.Quantity * i.Price);

    public void AddItem(string productId, string productName, decimal price, int quantity = 1)
    {
        var existing = Items.FirstOrDefault(i => i.ProductId == productId);
        if (existing != null)
            existing.Quantity += quantity;
        else
            Items.Add(new BasketItem(productId, productName, price, quantity) { ShoppingBasketId = this.Id });
    }

    public void RemoveItem(string productId)
    {
        Items.RemoveAll(i => i.ProductId == productId);
    }

    public void Clear() => Items.Clear();

    // 🔹 vigtig: bruges i BasketService til at erstatte hele kurven
    public void SetItems(List<BasketItem> items)
    {
        Items.Clear();
        foreach (var item in items)
        {
            item.ShoppingBasketId = this.Id; // sikrer FK er sat
            Items.Add(item);
        }
    }
}
