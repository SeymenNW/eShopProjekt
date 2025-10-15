using eShop.Basket.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace eShop.Basket.API.Tests;

public class BasketControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BasketControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostBasket_ShouldReturnCreated()
    {
        var basket = new ShoppingBasket
        {
            CustomerId = "test-customer",
            Items = new List<BasketItem>
            {
                new BasketItem { ProductId = "p01", ProductName = "Keyboard", Price = 250, Quantity = 1 }
            }
        };

        var response = await _client.PostAsJsonAsync("/api/basket", basket);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
