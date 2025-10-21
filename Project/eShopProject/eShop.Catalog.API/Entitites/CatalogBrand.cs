namespace eShop.Catalog.API.Entitites
{
    public class CatalogBrand
    {
        public string Brand { get; private set; }
        public CatalogBrand(string brand)
        {
            Brand = brand;
        }
    }
}
