using eShop.Catalog.API.Data;
using eShop.Catalog.API.Dto;
using eShop.Catalog.API.Entitites;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Catalog.API.Controllers
{
    [ApiController]
    [Route("/api/catalog-items")]
    public class CatalogItemController : Controller
    {

        private readonly CatalogDbContext _db;

        public CatalogItemController(CatalogDbContext db )
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetCatalogItems(int pageSize, int pageIndex, int catalogBrandId, int catalogTypeId)
        {
            try
            {
               var itemsList = _db.Items.ToList();

                return Ok(itemsList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("{catalogItemId:int}")]
        public IActionResult GetCatalogItemById(int catalogItemId)
        {
            return Ok("Adil skal købe mad");
        }

        [HttpDelete("{catalogItemId:int}")]
        public async Task<IActionResult> DeleteCatalogItems(int catalogItemId)
        {
            try
            {
                var item = _db.Items.First(item => item.Id == catalogItemId);
                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost()]
        public async Task<IActionResult> AddCatalogItem([FromBody] ItemDto catalogItemDto)
        {
            try
            {
                Item catalogItem = new Item
                {
                    Description = catalogItemDto.Description,
                    Price = catalogItemDto.Price,
                    PictureUri = catalogItemDto.PictureUri,
                    CatalogBrandId = catalogItemDto.CatalogBrandId,
                    CatalogTypeId = catalogItemDto.CatalogTypeId,
                    Name = catalogItemDto.Name,
                    
                };
                _db.Items.Add(catalogItem);
                await _db.SaveChangesAsync();
                return Ok($"Item: '{catalogItem.Description}' has been added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateCatalogItem([FromBody] Item catalogItem)
        {
            return Ok(catalogItem.Description);
        }


    }
}
