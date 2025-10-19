using eShop.Basket.Application.DTOs;

namespace eShop.Basket.Application.Interfaces;

public interface IBasketService
{
    Task<BasketDto?> GetAsync(string customerId);
    Task UpsertAsync(string customerId, IEnumerable<BasketItemDto> items);
    Task ClearAsync(string customerId);
    Task CheckoutAsync(string customerId);
}
