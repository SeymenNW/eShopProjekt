using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;


namespace eShop.Catalog.API.Endpoints;

/// <summary>
/// Get a Catalog Item by Id
/// </summary>
public class CatalogItemGetByIdEndpoint
{

    public void AddRoute(IEndpointRouteBuilder app)
    {
       
    }

    public async Task<IResult> HandleAsync()
    {
        throw new NotImplementedException();
    }
}
