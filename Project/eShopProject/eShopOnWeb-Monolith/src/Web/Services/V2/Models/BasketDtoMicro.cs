using Newtonsoft.Json;

namespace Microsoft.eShopWeb.Web.Services.V2.Models;

public class BasketDtoMicro
{
    [JsonProperty("customerId")]
    public string CustomerId { get; set; } = "";

    [JsonProperty("total")]
    public decimal Total { get; set; }

    [JsonProperty("items")]
    public List<BasketItemDtoMicro> Items { get; set; } = new();
}
