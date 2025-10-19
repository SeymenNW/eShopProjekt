namespace eShop.Basket.Application.DTOs;

public record BasketDto(Guid Id, string CustomerId, List<BasketItemDto> Items);
