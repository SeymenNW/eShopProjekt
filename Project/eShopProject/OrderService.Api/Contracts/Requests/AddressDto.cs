namespace OrderService.Api.Contracts.Requests
{
    public sealed class AddressDto
    {
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Zip { get; set; } = default!;
        public string Country { get; set; } = default!;
    }
}
