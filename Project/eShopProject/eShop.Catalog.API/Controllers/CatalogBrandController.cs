using Microsoft.AspNetCore.Mvc;

namespace eShop.Catalog.API.Controllers
{

    [ApiController]
<<<<<<< HEAD:Project/eShopProject/eShop.Catalog.API/Controllers/CatalogBrandController.cs
    [Route("api/catalog-brands")]
    public class CatalogBrandController : Controller
=======
    [Route("/api/[controller]")]
    public class CatalogController : Controller
>>>>>>> e6816056a63f71b46247bc252133f9fbbd72e1f4:Project/eShopProject/eShop.Catalog.API/Controllers/CatalogController.cs
    {
        [HttpGet]
        public async Task<IActionResult> GetCatalogBrands()
        {
            return Ok("Catalog API is running... Adam");
        }

     
    }
}
