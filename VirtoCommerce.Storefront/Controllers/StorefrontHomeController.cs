﻿using System.Threading.Tasks;
using System.Web.Mvc;
using PagedList;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Services;

namespace VirtoCommerce.Storefront.Controllers
{
    [OutputCache(CacheProfile = "HomeCachingProfile")]
    public class StorefrontHomeController : Controller
    {
        private readonly WorkContext _workContext;
        private readonly ICatalogSearchService _catalogSearchService;

        public StorefrontHomeController(WorkContext context, ICatalogSearchService catalogSearchService)
        {
            _workContext = context;
            _catalogSearchService = catalogSearchService;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var productSearchResult = await _catalogSearchService.SearchProductsAsync(new ProductSearchCriteria()
            {
                ResponseGroup = ItemResponseGroup.ItemSmall 
            });
            _workContext.Aggregations = new MutablePagedList<Aggregation>(productSearchResult.Aggregations);
            _workContext.CurrentCategory = new Category();
            _workContext.Products = new MutablePagedList<Product>(productSearchResult.Products);
            return View("index", _workContext);
        }
    }
}
