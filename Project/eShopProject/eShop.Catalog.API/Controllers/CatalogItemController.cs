using eShop.Catalog.API.Entitites;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Catalog.API.Controllers
{
    [ApiController]
    [Route("/api/catalog-items")]
    public class CatalogItemController : Controller
    {
        [HttpGet]
        public IActionResult GetCatalogItems(int pageSize, int pageIndex, int catalogBrandId, int catalogTypeId)
        {
            return Ok("Adil skal købe mad");
        }

        [HttpGet("{catalogItemId:int}")]
        public IActionResult GetCatalogItemById(int catalogItemId)
        {
            return Ok("Adil skal købe mad");
        }

        [HttpDelete("{catalogItemId:int}")]
        public async Task<IActionResult> DeleteCatalogItems(int catalogItemId)
        {
            return Ok($"Id: {catalogItemId} ");
        }

        [HttpPost()]
        public async Task<IActionResult> AddCatalogItem([FromBody] CatalogItem catalogItem)
        {
            return Ok(catalogItem.Description);
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateCatalogItem([FromBody] CatalogItem catalogItem)
        {
            return Ok(catalogItem.Description);
        }


    }
}
