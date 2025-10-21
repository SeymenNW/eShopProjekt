using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using MinimalApi.Endpoint;

namespace eShop.Catalog.API.Endpoints;


/// <summary>
/// List Catalog Items (paged)
/// </summary>
public class CatalogItemListPagedEndpoint
{


    public void AddRoute(IEndpointRouteBuilder app)
    {
      
    }

    public async Task<IResult> HandleAsync()
    {
        throw new NotImplementedException();
    }
}
