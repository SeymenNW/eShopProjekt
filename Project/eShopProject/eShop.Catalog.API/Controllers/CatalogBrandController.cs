using Microsoft.AspNetCore.Mvc;

namespace eShop.Catalog.API.Controllers
{

    [ApiController]
    [Route("api/catalog-brands")]
    public class CatalogBrandController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetCatalogBrands()
        {
            return Ok("Catalog API is running... Adam");
        }

     
    }
}
