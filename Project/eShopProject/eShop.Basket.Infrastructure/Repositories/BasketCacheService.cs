using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using eShop.Basket.Domain.Entities;

namespace eShop.Basket.Infrastructure.Repositories;

public class BasketCacheService
{
    private readonly IDistributedCache _cache;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public BasketCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    private string GetKey(string customerId) => $"basket:{customerId}";

    public async Task<ShoppingBasket?> GetBasketAsync(string customerId)
    {
        var data = await _cache.GetStringAsync(GetKey(customerId));
        return data is null ? null : JsonSerializer.Deserialize<ShoppingBasket>(data, _jsonOptions);
    }

    public async Task SaveBasketAsync(ShoppingBasket basket)
    {
        var json = JsonSerializer.Serialize(basket, _jsonOptions);
        await _cache.SetStringAsync(GetKey(basket.CustomerId), json);
    }

    public async Task DeleteBasketAsync(string customerId)
        => await _cache.RemoveAsync(GetKey(customerId));
}
