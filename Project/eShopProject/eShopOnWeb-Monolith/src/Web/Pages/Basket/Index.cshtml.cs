using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.Web.Services.V2;
using Microsoft.eShopWeb.Web.ViewModels;

namespace Microsoft.eShopWeb.Web.Pages.Basket;

public class IndexModel : PageModel
{
    private readonly BasketMicroserviceViewModelService _basketMicro;

    public BasketViewModel BasketModel { get; private set; } = new();

    public IndexModel(BasketMicroserviceViewModelService basketMicro)
    {
        _basketMicro = basketMicro;
    }

    public async Task OnGetAsync()
    {
        var buyerId = "cust-1003"; // test hardcoded
        BasketModel = await _basketMicro.GetBasketAsync(buyerId);
    }

    // 🔹 Denne håndterer "Update"-knappen
    public async Task<IActionResult> OnPostUpdateAsync(IEnumerable<BasketItemViewModel> items)
    {
        var buyerId = "cust-1003"; // eller hent fra cookie i GetOrSetBasketCookieAndUserName()

        if (!ModelState.IsValid)
            return Page();

        // Lav et nyt BasketViewModel baseret på input
        var updatedBasket = new BasketViewModel
        {
            BuyerId = buyerId,
            Items = items.ToList()
        };

        // Send ændringer til microservice (Basket.API via Gateway)
        await _basketMicro.UpdateBasketAsync(updatedBasket);

        // Hent opdateret kurv igen for at vise korrekte cost/total
        BasketModel = await _basketMicro.GetBasketAsync(buyerId);

        // Genindlæs siden for at vise de nye værdier
        return RedirectToPage();
    }

    private string GetOrSetBasketCookieAndUserName()
    {
        if (Request.HttpContext.User.Identity?.IsAuthenticated == true)
            return Request.HttpContext.User.Identity.Name!;

        if (Request.Cookies.ContainsKey(Constants.BASKET_COOKIENAME))
            return Request.Cookies[Constants.BASKET_COOKIENAME]!;

        var userName = Guid.NewGuid().ToString();
        var cookieOptions = new CookieOptions { IsEssential = true, Expires = DateTime.Today.AddYears(10) };
        Response.Cookies.Append(Constants.BASKET_COOKIENAME, userName, cookieOptions);
        return userName;
    }
}
