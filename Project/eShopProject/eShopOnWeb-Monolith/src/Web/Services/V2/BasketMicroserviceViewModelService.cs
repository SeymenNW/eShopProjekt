using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.eShopWeb.Web.Pages.Basket;
using Microsoft.eShopWeb.Web.Services.V2.Models;
using Microsoft.eShopWeb.Web.ViewModels;
using Newtonsoft.Json;

namespace Microsoft.eShopWeb.Web.Services.V2;

public class BasketMicroserviceViewModelService
{
    private readonly HttpClient _http;

    public BasketMicroserviceViewModelService(HttpClient http)
    {
        _http = http ?? throw new ArgumentNullException(nameof(http));
    }

    public async Task<BasketViewModel> GetBasketAsync(string buyerId)
    {
        var response = await _http.GetAsync($"api/Basket/{buyerId}");
        if (!response.IsSuccessStatusCode)
            return new BasketViewModel { BuyerId = buyerId, Items = new() };

        var dto = JsonConvert.DeserializeObject<BasketDtoMicro>(
            await response.Content.ReadAsStringAsync());

        if (dto == null)
            return new BasketViewModel { BuyerId = buyerId, Items = new() };

        var vm = new BasketViewModel
        {
            BuyerId = dto.CustomerId,
            Items = dto.Items.Select(i => new BasketItemViewModel
            {
                CatalogItemId = int.TryParse(i.ProductId, out var id) ? id : 0,
                ProductName = i.ProductName,
                UnitPrice = i.Price,
                Quantity = i.Quantity,
                PictureUrl = i.PictureUrl
            }).ToList()
        };

        return vm;
    }

    // ✅ Denne mangler hos dig — den fikser 400-fejlen
    public async Task UpdateBasketAsync(BasketViewModel basket)
    {
        if (basket == null)
            throw new ArgumentNullException(nameof(basket));

        var dto = new BasketDtoMicro
        {
            CustomerId = basket.BuyerId ?? string.Empty,
            Items = basket.Items?
                .Select(i => new BasketItemDtoMicro
                {
                    ProductId = i.CatalogItemId.ToString(),
                    ProductName = i.ProductName ?? string.Empty,
                    Price = i.UnitPrice,
                    Quantity = i.Quantity,
                    PictureUrl = i.PictureUrl ?? string.Empty
                })
                .ToList() ?? new List<BasketItemDtoMicro>()
        };

        var json = JsonConvert.SerializeObject(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _http.PostAsync("api/Basket", content);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();
            throw new Exception($"Basket update failed: {(int)response.StatusCode} {response.ReasonPhrase}\n{body}");
        }
    }

}
