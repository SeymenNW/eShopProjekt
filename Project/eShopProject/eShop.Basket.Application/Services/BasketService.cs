using eShop.Basket.Application.DTOs;
using eShop.Basket.Application.Interfaces;
using eShop.Basket.Domain.Entities;
using eShop.BuildingBlocks.EventBus;
using eShop.BuildingBlocks.EventBus.Events;
using eShop.Basket.Infrastructure.Data;
using eShop.Basket.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eShop.Basket.Application.Services;

public class BasketService : IBasketService
{
    private readonly BasketDbContext _db;
    private readonly IEventBus _bus;
    private readonly BasketCacheService _cache;

    public BasketService(BasketDbContext db, IEventBus bus, BasketCacheService cache)
    {
        _db = db;
        _bus = bus;
        _cache = cache;
    }

    public async Task<BasketDto?> GetAsync(string customerId)
    {
        // 1️⃣ Prøv Redis først
        var cached = await _cache.GetBasketAsync(customerId);
        if (cached != null)
        {
            return new BasketDto(
                cached.Id,
                cached.CustomerId,
                cached.Items.Select(i =>
                    new BasketItemDto(i.ProductId, i.ProductName, i.Price, i.Quantity)).ToList());
        }

        // 2️⃣ Hvis ikke i cache → hent fra DB
        var basket = await _db.Baskets
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.CustomerId == customerId);
        if (basket == null) return null;

        // 3️⃣ Gem i Redis for hurtig adgang
        await _cache.SaveBasketAsync(basket);

        return new BasketDto(
            basket.Id,
            basket.CustomerId,
            basket.Items.Select(i =>
                new BasketItemDto(i.ProductId, i.ProductName, i.Price, i.Quantity)).ToList());
    }

    public async Task UpsertAsync(string customerId, IEnumerable<BasketItemDto> items)
    {
        // 1️⃣ Hent fra Redis hvis muligt
        var basket = await _cache.GetBasketAsync(customerId);

        // 2️⃣ Hvis ikke i cache → hent fra DB
        if (basket == null)
        {
            basket = await _db.Baskets.Include(b => b.Items)
                .FirstOrDefaultAsync(b => b.CustomerId == customerId);

            // 3️⃣ Hvis ingen kurv → opret ny og gem for at få Id
            if (basket == null)
            {
                basket = new ShoppingBasket { CustomerId = customerId };
                _db.Baskets.Add(basket);
                await _db.SaveChangesAsync(); // 🔹 her genereres basket.Id i DB
            }
        }

        // 4️⃣ Opret varer med korrekt foreign key
        var entityItems = items.Select(i => new BasketItem
        {
            ProductId = i.ProductId,
            ProductName = i.ProductName,
            Price = i.Price,
            Quantity = i.Quantity,
            ShoppingBasketId = basket.Id // 🔹 FK sættes her
        }).ToList();

        basket.SetItems(entityItems);

        // 5️⃣ Gem i DB og Redis
        _db.Update(basket);
        await _db.SaveChangesAsync();
        await _cache.SaveBasketAsync(basket);
    }

    public async Task ClearAsync(string customerId)
    {
        var basket = await _db.Baskets.FirstOrDefaultAsync(b => b.CustomerId == customerId);
        if (basket == null) return;

        _db.Baskets.Remove(basket);
        await _db.SaveChangesAsync();
        await _cache.DeleteBasketAsync(customerId);
    }

    public async Task CheckoutAsync(string customerId)
    {
        var basket = await _cache.GetBasketAsync(customerId)
            ?? await _db.Baskets.Include(b => b.Items)
                .FirstOrDefaultAsync(b => b.CustomerId == customerId);

        if (basket == null) return;

        var total = basket.Items.Sum(i => i.Price * i.Quantity);

        _bus.Publish("basket.checkedout",
            new BasketCheckedOutIntegrationEvent(basket.Id, customerId, total));

        _db.Baskets.Remove(basket);
        await _db.SaveChangesAsync();
        await _cache.DeleteBasketAsync(customerId);
    }
}
