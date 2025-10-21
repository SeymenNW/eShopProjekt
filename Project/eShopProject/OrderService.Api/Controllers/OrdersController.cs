using Microsoft.AspNetCore.Mvc;
using OrderService.Api.Contracts.Requests;
using OrderService.Api.Contracts.Responses;
using OrderService.Domain.Abstractions;
using OrderService.Domain.Models;

namespace OrderService.Api.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public sealed class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _repo;
        public OrdersController(IOrderRepository repo) => _repo = repo;

        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create(CreateOrderRequest r, CancellationToken ct)
        {
            // Check if unitPrice and units are valid for each item
            foreach (var item in r.Items)
            {
                if (item.UnitPrice <= 0)
                {
                    return BadRequest($"Unit price for item {item.Name} must be greater than 0.");
                }

                if (item.Units <= 0)
                {
                    return BadRequest($"Units for item {item.Name} must be greater than 0.");
                }
            }

            // Create order and add items
            var order = new Order(r.BuyerId, new Address(r.ShipTo.Street, r.ShipTo.City, r.ShipTo.Zip, r.ShipTo.Country));

            foreach (var it in r.Items)
            {
                var snap = new ItemSnapshot(it.ItemId, it.Name, it.PictureUri);
                order.AddItem(snap, it.UnitPrice, it.Units);
            }

            // Save order to the repository
            await _repo.AddAsync(order, ct);
            await _repo.SaveChangesAsync(ct);

            return CreatedAtAction(nameof(GetById), new { id = order.Id }, ToDto(order));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDto>> GetById(int id, CancellationToken ct)
        {
            var o = await _repo.GetAsync(id, ct);
            return o is null ? NotFound() : Ok(ToDto(o));
        }

        private static OrderDto ToDto(Order o) => new()
        {
            Id = o.Id,
            BuyerId = o.BuyerId,
            OrderDate = o.OrderDate,
            Status = o.Status.ToString(),
            Total = o.Total(),
            Items = o.Items.Select(i => new OrderItemDto
            {
                ItemId = i.Item.ItemId,
                Name = i.Item.Name,
                PictureUri = i.Item.PictureUri,
                UnitPrice = i.UnitPrice,
                Units = i.Units,
                LineTotal = i.LineTotal
            }).ToList()
        };
    }
}
