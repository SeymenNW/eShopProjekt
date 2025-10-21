namespace OrderService.Api.Contracts.Requests
{
    public sealed class CreateOrderItemDto
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; } = default!;
        public string? PictureUri { get; set; }
        public decimal UnitPrice { get; set; }
        public int Units { get; set; }
    }
}
