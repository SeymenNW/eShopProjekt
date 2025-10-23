using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.Web.Services.V2.Models;
using Microsoft.eShopWeb.Web.ViewModels;

namespace Microsoft.eShopWeb.Web.Services.V2;

public class CatalogViewModelServiceMicro : ICatalogViewModelService
{
    private readonly HttpClient _httpClient;
    public CatalogViewModelServiceMicro(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<IEnumerable<SelectListItem>> GetBrands()
    {
        throw new NotImplementedException();
    }

    public async Task<CatalogIndexViewModel> GetCatalogItems(int pageIndex, int itemsPage, int? brandId, int? typeId)
    {

        return new CatalogIndexViewModel
        {
            CatalogItems = await GetCatalogItemsAsync(),
            Brands = [],
            

        };
    }

    public Task<IEnumerable<SelectListItem>> GetTypes()
    {
        throw new NotImplementedException();
    }

    private async Task<List<CatalogItemViewModel>> GetCatalogItemsAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("");

        if (response.IsSuccessStatusCode)
        {
          var content = JsonSerializer.Deserialize<List<CatalogItemDtoMicro>>(await response.Content.ReadAsStringAsync());

            List<CatalogItemViewModel> items = new();

            foreach (var item in content)
            {
                items.Add(new CatalogItemViewModel { Name = item.Name, Id = item.Id, PictureUri = item.PictureUri, Price = item.Price });
            }

            return items;

        } else
        {
            return [];
        }

    }

    private async Task<List<CatalogItemViewModel>> GetBrandsAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("");

        if (response.IsSuccessStatusCode)
        {
            var content = JsonSerializer.Deserialize<List<CatalogItemDtoMicro>>(await response.Content.ReadAsStringAsync());

            List<CatalogItemViewModel> items = new();

            foreach (var item in content)
            {
                items.Add(new CatalogItemViewModel { Name = item.Name, Id = item.Id, PictureUri = item.PictureUri, Price = item.Price });
            }

            return items;

        }
        else
        {
            return [];
        }

    }



}
