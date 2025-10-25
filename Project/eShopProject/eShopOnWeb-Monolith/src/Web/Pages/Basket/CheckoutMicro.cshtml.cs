using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.Web.Services.V2;
using Microsoft.eShopWeb.Web.ViewModels;

namespace Microsoft.eShopWeb.Web.Pages.Basket;

public class CheckoutMicroModel : PageModel
{
    private readonly BasketMicroserviceViewModelService _basketMicro;

    public BasketViewModel BasketModel { get; private set; } = new();

    public CheckoutMicroModel(BasketMicroserviceViewModelService basketMicro)
    {
        _basketMicro = basketMicro;
    }

    public async Task OnGetAsync()
    {
        var buyerId = GetOrSetBasketCookieAndUserName();
        BasketModel = await _basketMicro.GetBasketAsync(buyerId);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var buyerId = GetOrSetBasketCookieAndUserName();
        await _basketMicro.CheckoutBasketAsync(buyerId);
        return RedirectToPage("Success");
    }

    private string GetOrSetBasketCookieAndUserName()
    {
        if (User.Identity?.IsAuthenticated == true)
            return User.Identity.Name!;

        if (Request.Cookies.TryGetValue(Constants.BASKET_COOKIENAME, out var id))
            return id;

        var userName = Guid.NewGuid().ToString();
        Response.Cookies.Append(Constants.BASKET_COOKIENAME, userName,
            new CookieOptions { IsEssential = true, Expires = DateTime.Today.AddYears(10) });
        return userName;
    }
}
