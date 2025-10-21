namespace eShop.Catalog.API.Entitites
{
    public class CatalogType
    {
        public string Type { get; private set; }
        public CatalogType(string type)
        {
            Type = type;
        }
    }
}
