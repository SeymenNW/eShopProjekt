using eShop.Basket.Domain.Entities;
using eShop.Basket.Infrastructure.Data;
using eShop.Basket.Infrastructure.EventBus;
using eShop.Basket.Domain.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShop.Basket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BasketController : ControllerBase
{
    private readonly BasketDbContext _context;
    private readonly IEventBus _eventBus;

    private const string BasketCheckedOutQueue = "basket.checkedout";

    public BasketController(BasketDbContext context, IEventBus eventBus)
    {
        _context = context;
        _eventBus = eventBus;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ShoppingBasket>> GetBasket(Guid id)
    {
        var basket = await _context.Baskets
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.Id == id);

        return basket == null ? NotFound() : Ok(basket);
    }

    [HttpPost]
    public async Task<ActionResult<ShoppingBasket>> CreateBasket([FromBody] ShoppingBasket basket)
    {
        basket.Id = Guid.NewGuid();
        _context.Baskets.Add(basket);
        await _context.SaveChangesAsync();

        var @event = new BasketCheckedOutIntegrationEvent(
            basket.Id,
            basket.CustomerId,
            basket.TotalPrice
        );

        _eventBus.Publish(BasketCheckedOutQueue, @event);
        Console.WriteLine($"[Publish] BasketCheckedOut event sent for Basket {@event.BasketId}");

        return CreatedAtAction(nameof(GetBasket), new { id = basket.Id }, basket);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBasket(Guid id)
    {
        var basket = await _context.Baskets.FindAsync(id);
        if (basket == null)
            return NotFound();

        _context.Baskets.Remove(basket);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
