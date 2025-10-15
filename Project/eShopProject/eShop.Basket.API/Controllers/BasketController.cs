using eShop.Basket.Domain.Entities;
using eShop.Basket.Infrastructure.Data;
using eShop.Basket.Infrastructure.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace eShop.Basket.API.Controllers;
//[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BasketController : ControllerBase
{
    private readonly BasketDbContext _context;

    public BasketController(BasketDbContext context)
    {
        _context = context;
    }

    // GET api/basket/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ShoppingBasket>> GetBasket(Guid id)
    {
        var basket = await _context.Baskets
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (basket == null)
            return NotFound();

        return Ok(basket);
    }

    // POST api/basket
    [HttpPost]
    public async Task<ActionResult<ShoppingBasket>> CreateBasket([FromBody] ShoppingBasket basket)
    {
        basket.Id = Guid.NewGuid();
        _context.Baskets.Add(basket);
        await _context.SaveChangesAsync();
        var client = new RabbitMqClient();
        client.Publish("basket.checkedout", new
        {
            BasketId = basket.Id,
            CustomerId = basket.CustomerId,
            Total = basket.TotalPrice,
            Date = DateTime.UtcNow
        });
        return CreatedAtAction(nameof(GetBasket), new { id = basket.Id }, basket);
    }

    // DELETE api/basket/{id}
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
