namespace Microsoft.eShopWeb.Web.Services.V2.Models;

public class CatalogItemDtoMicro
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string PictureUri { get; set; }
    public int CatalogTypeId { get; set; }
    public int CatalogBrandId { get; set; }

}
