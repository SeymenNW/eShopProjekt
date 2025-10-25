using System;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.Infrastructure.Identity;
using Microsoft.eShopWeb.Web.Services.V2;
using Microsoft.eShopWeb.Web.ViewModels;

namespace Microsoft.eShopWeb.Web.Pages.Shared.Components.BasketComponent;

public class Basket : ViewComponent
{
    private readonly BasketMicroserviceViewModelService _basketMicro;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public Basket(BasketMicroserviceViewModelService basketMicro,
                  SignInManager<ApplicationUser> signInManager)
    {
        _basketMicro = basketMicro;
        _signInManager = signInManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var buyerId = GetBuyerId();
        var basket = await _basketMicro.GetBasketAsync(buyerId);

        var vm = new BasketComponentViewModel
        {
            ItemsCount = basket?.Items?.Sum(i => i.Quantity) ?? 0
        };

        return View(vm);
    }

    private string GetBuyerId()
    {
        if (_signInManager.IsSignedIn(HttpContext.User) &&
            !string.IsNullOrEmpty(User?.Identity?.Name))
        {
            return User.Identity.Name!;
        }

        if (Request.Cookies.TryGetValue(Constants.BASKET_COOKIENAME, out var cookieId))
        {
            return cookieId;
        }

        var newId = Guid.NewGuid().ToString();
        HttpContext.Response.Cookies.Append(Constants.BASKET_COOKIENAME, newId,
        new CookieOptions { IsEssential = true, Expires = DateTime.Today.AddYears(10) });
        return newId;
    }
}
