using Newtonsoft.Json;

namespace Microsoft.eShopWeb.Web.Services.V2;

public class BasketItemDtoMicro
{
    [JsonProperty("productId")]
    public string ProductId { get; set; } = "";

    [JsonProperty("productName")]
    public string ProductName { get; set; } = "";

    [JsonProperty("quantity")]
    public int Quantity { get; set; }

    [JsonProperty("price")]
    public decimal Price { get; set; }

    [JsonProperty("pictureUrl")]
    public string PictureUrl { get; set; } = "";
}
