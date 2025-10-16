namespace eShop.Basket.Domain.Events;

public class BasketCheckedOutIntegrationEvent : IntegrationEvent
{
    public Guid BasketId { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public DateTime Date { get; set; }

    public BasketCheckedOutIntegrationEvent(Guid basketId, string customerId, decimal total)
    {
        BasketId = basketId;
        CustomerId = customerId;
        Total = total;
        Date = DateTime.UtcNow;
    }
}
