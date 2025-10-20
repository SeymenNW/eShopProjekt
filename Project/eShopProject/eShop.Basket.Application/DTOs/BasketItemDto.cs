namespace eShop.Basket.Application.DTOs;

public record BasketItemDto(string ProductId, string ProductName, decimal Price, int Quantity);
