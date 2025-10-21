using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;


namespace eShop.Catalog.API.Endpoints;

/// <summary>
/// Deletes a Catalog Item
/// </summary>
public class DeleteCatalogItemEndpoint 
{
    public void AddRoute(IEndpointRouteBuilder app)
    {
       
    }

    public async Task<IResult> HandleAsync()
    {
      throw new NotImplementedException();
    }
}
