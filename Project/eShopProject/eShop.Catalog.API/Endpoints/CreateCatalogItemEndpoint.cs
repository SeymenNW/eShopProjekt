using System.Threading.Tasks;
using eShop.Catalog.API.Entitites;
using eShop.Catalog.API.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using MinimalApi.Endpoint;

namespace eShop.Catalog.API.Endpoints;

/// <summary>
/// Creates a new Catalog Item
/// </summary>
public class CreateCatalogItemEndpoint
{


    public void AddRoute(IEndpointRouteBuilder app)
    {
    
    }

    public async Task<IResult> HandleAsync()
    {
        throw new NotImplementedException();    
       
    }
}
