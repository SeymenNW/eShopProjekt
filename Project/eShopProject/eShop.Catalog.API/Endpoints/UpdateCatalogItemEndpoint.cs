using System.Threading.Tasks;
using eShop.Catalog.API.Entitites;
using eShop.Catalog.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints;


namespace eShop.Catalog.API.Endpoints;

/// <summary>
/// Updates a Catalog Item
/// </summary>
public class UpdateCatalogItemEndpoint
{
    public void AddRoute(IEndpointRouteBuilder app)
    {

    }

    public async Task<IResult> HandleAsync()
    {
        throw new NotImplementedException();
    }

}
