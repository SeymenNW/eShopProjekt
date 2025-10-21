    using System.ComponentModel.DataAnnotations.Schema;

namespace eShop.Catalog.API.Entitites
{
    public partial class Brand
    {
        public int Id { get; set; }

        public string? BrandName { get; set; }

    }
}
