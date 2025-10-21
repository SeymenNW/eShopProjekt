namespace OrderService.Api.Contracts.Requests
{
    public sealed class CreateOrderRequest
    {
        public string BuyerId { get; set; } = default!;
        public AddressDto ShipTo { get; set; } = default!;
        public List<CreateOrderItemDto> Items { get; set; } = new();
    }
}
