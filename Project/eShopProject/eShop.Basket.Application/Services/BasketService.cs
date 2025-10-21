using eShop.Basket.Application.DTOs;
using eShop.Basket.Application.Interfaces;
using eShop.Basket.Domain.Entities;
using eShop.Basket.Domain.Events;
using eShop.Basket.Infrastructure.Data;
using eShop.Basket.Infrastructure.EventBus;
using Microsoft.EntityFrameworkCore;

namespace eShop.Basket.Application.Services;

public class BasketService : IBasketService
{
    private readonly BasketDbContext _db;
    private readonly IEventBus _bus;

    public BasketService(BasketDbContext db, IEventBus bus)
    {
        _db = db;
        _bus = bus;
    }

    public async Task<BasketDto?> GetAsync(string customerId)
        => await _db.Baskets
            .Include(b => b.Items)
            .Where(b => b.CustomerId == customerId)
            .Select(b => new BasketDto(
                b.Id,
                b.CustomerId,
                b.Items.Select(i =>
                    new BasketItemDto(i.ProductId, i.ProductName, i.Price, i.Quantity)).ToList()))
            .FirstOrDefaultAsync();

    public async Task UpsertAsync(string customerId, IEnumerable<BasketItemDto> items)
    {
        var basket = await _db.Baskets
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.CustomerId == customerId);

        // Hvis kunden ikke har kurv endnu → opret ny og gem for at få Id
        if (basket == null)
        {
            basket = new ShoppingBasket
            {
                CustomerId = customerId
            };
            _db.Baskets.Add(basket);
            await _db.SaveChangesAsync(); // 🔹 her får basket.Id værdi
        }

        // Opret varer med korrekt FK
        var entityItems = items.Select(i => new BasketItem
        {
            ProductId = i.ProductId,
            ProductName = i.ProductName,
            Price = i.Price,
            Quantity = i.Quantity,
            ShoppingBasketId = basket.Id // 🔹 vigtigt for FK
        }).ToList();

        basket.SetItems(entityItems);

        _db.Update(basket);
        await _db.SaveChangesAsync();
    }


    public async Task ClearAsync(string customerId)
    {
        var basket = await _db.Baskets.FirstOrDefaultAsync(b => b.CustomerId == customerId);
        if (basket == null) return;

        _db.Baskets.Remove(basket);
        await _db.SaveChangesAsync();
    }

    public async Task CheckoutAsync(string customerId)
    {
        var basket = await _db.Baskets
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.CustomerId == customerId);
        if (basket == null) return;

        var total = basket.Items.Sum(i => i.Price * i.Quantity);

        _bus.Publish("basket.checkedout",
            new BasketCheckedOutIntegrationEvent(basket.Id, customerId, total));

        _db.Baskets.Remove(basket);
        await _db.SaveChangesAsync();
    }
}
