using eShop.Basket.Application.DTOs;
using eShop.Basket.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Basket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketService _service;

    public BasketController(IBasketService service)
    {
        _service = service;
    }

    [HttpGet("{customerId}")]
    public async Task<ActionResult<BasketDto?>> GetBasket(string customerId)
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Basket API is running... Adam");
    }

    // GET api/basket/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ShoppingBasket>> GetBasket(Guid id)
    {
        var basket = await _service.GetAsync(customerId);
        return basket is null ? NotFound() : Ok(basket);
    }

    [HttpPost]
    public async Task<ActionResult> UpsertBasket([FromBody] BasketUpdateRequest request)
    {
        await _service.UpsertAsync(request.CustomerId, request.Items);
        return Ok();
    }

    [HttpDelete("{customerId}")]
    public async Task<ActionResult> ClearBasket(string customerId)
    {
        await _service.ClearAsync(customerId);
        return NoContent();
    }

    [HttpPost("checkout")]
    public async Task<ActionResult> Checkout([FromBody] CheckoutRequest request)
    {
        await _service.CheckoutAsync(request.CustomerId);
        return Accepted();
    }
}

// Request DTOs
public record BasketUpdateRequest(string CustomerId, IEnumerable<BasketItemDto> Items);
public record CheckoutRequest(string CustomerId);
