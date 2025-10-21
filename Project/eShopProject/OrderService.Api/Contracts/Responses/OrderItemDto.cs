namespace OrderService.Api.Contracts.Responses
{
    public sealed class OrderItemDto
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; } = default!;
        public string? PictureUri { get; set; }
        public decimal UnitPrice { get; set; }
        public int Units { get; set; }
        public decimal LineTotal { get; set; }
    }
}
