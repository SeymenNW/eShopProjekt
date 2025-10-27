using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using Microsoft.eShopWeb.Web.Services.V2.Models;
using Microsoft.eShopWeb.Web.ViewModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Microsoft.eShopWeb.Web.Services;

/// <summary>
/// This is a UI-specific service so belongs in UI project. It does not contain any business logic and works
/// with UI-specific types (view models and SelectListItem types).
/// </summary>
public class CatalogViewModelService : ICatalogViewModelService
{
    private readonly ILogger<CatalogViewModelService> _logger;
    private readonly IRepository<CatalogItem> _itemRepository;
    private readonly IRepository<CatalogBrand> _brandRepository;
    private readonly IRepository<CatalogType> _typeRepository;
    private readonly IUriComposer _uriComposer;
    private readonly HttpClient _httpClient;

    public CatalogViewModelService(
        HttpClient httpClient,
        ILoggerFactory loggerFactory,
        IRepository<CatalogItem> itemRepository,
        IRepository<CatalogBrand> brandRepository,
        IRepository<CatalogType> typeRepository,
        IUriComposer uriComposer)
    {
        _logger = loggerFactory.CreateLogger<CatalogViewModelService>();
        _itemRepository = itemRepository;
        _brandRepository = brandRepository;
        _typeRepository = typeRepository;
        _uriComposer = uriComposer;
        _httpClient = httpClient;
    }

    public async Task<CatalogIndexViewModel> GetCatalogItems(int pageIndex, int itemsPage, int? brandId, int? typeId)
    {
        _logger.LogInformation("GetCatalogItems called.");

        var filterSpecification = new CatalogFilterSpecification(brandId, typeId);
        var filterPaginatedSpecification =
            new CatalogFilterPaginatedSpecification(itemsPage * pageIndex, itemsPage, brandId, typeId);

        // the implementation below using ForEach and Count. We need a List.
        var itemsOnPage = await GetCatalogItemsAsync();
        var totalItems = itemsOnPage.Count;

        var vm = new CatalogIndexViewModel()
        {
            CatalogItems = itemsOnPage,
            Brands = (await GetBrands()).ToList(),
            Types = (await GetTypes()).ToList(),
            BrandFilterApplied = brandId ?? 0,
            TypesFilterApplied = typeId ?? 0,
            PaginationInfo = new PaginationInfoViewModel()
            {
                ActualPage = pageIndex,
                ItemsPerPage = itemsOnPage.Count,
                TotalItems = totalItems,
                TotalPages = int.Parse(Math.Ceiling(((decimal)totalItems / itemsPage)).ToString())
            }
        };

        vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
        vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";

        return vm;
    }

    public async Task<IEnumerable<SelectListItem>> GetBrands()
    {
        _logger.LogInformation("GetBrands called.");
        var brands = await GetCatalogBrandsAsync();

        var items = brands
            .Select(brand => new SelectListItem() { Value = brand.Id.ToString(), Text = brand.Brand })
            .OrderBy(b => b.Text)
            .ToList();

        var allItem = new SelectListItem() { Value = null, Text = "All", Selected = true };
        items.Insert(0, allItem);

        return items;
    }

    public async Task<IEnumerable<SelectListItem>> GetTypes()
    {
        _logger.LogInformation("GetTypes called.");
        var types = await GetCatalogTypesAsync();

        var items = types
            .Select(type => new SelectListItem() { Value = type.Id.ToString(), Text = type.Type })
            .OrderBy(t => t.Text)
            .ToList();

        var allItem = new SelectListItem() { Value = null, Text = "All", Selected = true };
        items.Insert(0, allItem);

        return items;
    }

    private async Task<List<CatalogItemViewModel>> GetCatalogItemsAsync()
    {
        HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5025/api/catalog-items");

        if (response.IsSuccessStatusCode)
        {
            var content = JsonConvert.DeserializeObject<List<CatalogItemDtoMicro>>(await response.Content.ReadAsStringAsync());

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

    private async Task<List<CatalogBrand>> GetCatalogBrandsAsync()
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5025/api/catalog-brands");

            if (response.IsSuccessStatusCode)
            {
                var brandItems = JsonConvert.DeserializeObject<List<CatalogBrandDtoMicro>>(await response.Content.ReadAsStringAsync());


               List<CatalogBrand> items = new();

                foreach (var item in brandItems)
                {
                    CatalogBrand catalogBrand = new(item.BrandName);
                    items.Add(catalogBrand);
                }

                return items;

            }
            else
            {
                return [];
            }
        }
        catch (Exception ex)
        {
            return [];
        }
    }

    private async Task<List<CatalogType>> GetCatalogTypesAsync()
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5025/api/catalog-types");

            if (response.IsSuccessStatusCode)
            {
                var brandItems = JsonConvert.DeserializeObject<List<CatalogTypeDtoMicro>>(await response.Content.ReadAsStringAsync());


                List<CatalogType> items = new();

                foreach (var item in brandItems)
                {
                    items.Add(new CatalogType(item.TypeName));
                }

                return items;

            }
            else
            {
                return [];
            }
        }
        catch (Exception ex)
        {
            return [];
        }
    }

}
