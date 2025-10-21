namespace OrderService.Api.Contracts.Responses
{
    public sealed class OrderDto
    {
        public int Id { get; set; }
        public string BuyerId { get; set; } = default!;
        public DateTimeOffset OrderDate { get; set; }
        public string Status { get; set; } = default!;
        public decimal Total { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
