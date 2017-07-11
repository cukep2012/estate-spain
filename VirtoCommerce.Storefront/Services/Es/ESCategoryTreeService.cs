﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.AutoRestClients.SearchApiModuleApi;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Services.Es.Converters;
using VirtoCommerce.Storefront.Converters;
using VirtoCommerce.Storefront.Model.Stores;
using VirtoCommerce.Storefront.Model;
using System.Threading;
using System.Configuration;

namespace VirtoCommerce.Storefront.Services.Es
{
    public class ESCategoryTreeService: ICategoryTreeService
    {
        private const string _storeId = "MasterStore";
        private readonly SemaphoreSlim _lockObject = new SemaphoreSlim(1);
        private readonly ISearchApiModuleApiClient _searchApi;
        private readonly Func<WorkContext> _workContextFactory;
        private static Category _loadedCategory;
        private static Dictionary<string, Category> _seoCategoryDict = new Dictionary<string, Category>();
        private Language _language;
        private Currency _currency;
        private Store _store;

        public ESCategoryTreeService(ISearchApiModuleApiClient searchApi, Func<WorkContext> workContextFactory)
        {
            _searchApi = searchApi;
            _workContextFactory = workContextFactory;
        }

        public async Task<Category> GetTree()
        {
            await _lockObject.WaitAsync();
            if (_loadedCategory != null)
            {
                _lockObject.Release();
                return _loadedCategory;
            }
            _loadedCategory = new Category();
            // init data for product converter
            var wc = _workContextFactory();
            _language = wc.CurrentLanguage;
            _currency = wc.CurrentCurrency;
            _store = wc.CurrentStore;
            // Load links for regions
            //await LoadChildrenFromCategory(new Category[] { new Category() }, "Regions");
            // Load region and regio + estate type
            await LoadChildrenFromCategory(new Category[] { new Category() }, "Regions").ContinueWith(t => LoadChildrenFromCategory(t.Result, "Estatetypes"));
            // Load region + tags
            await LoadChildrenFromCategory(new Category[] { new Category() }, "Regions").ContinueWith(t => LoadChildrenFromCategory(t.Result, "Tags"));
            // Load region + condition
            await LoadChildrenFromCategory(new Category[] { new Category() }, "Regions").ContinueWith(t => LoadChildrenFromCategory(t.Result, "Conditions"));
            // Load region + estate type + tags
            await LoadChildrenFromCategory(new Category[] { new Category() }, "Regions").ContinueWith(t => LoadChildrenFromCategory(t.Result, "Estatetypes").ContinueWith(t1 => LoadChildrenFromCategory(t1.Result, "Tags")));
            // Load region + estate type + condition
            await LoadChildrenFromCategory(new Category[] { new Category() }, "Regions").ContinueWith(t => LoadChildrenFromCategory(t.Result, "Estatetypes").ContinueWith(t1 => LoadChildrenFromCategory(t1.Result, "Conditions")));

            // Load cities + estate type + tags
            await LoadChildrenFromCategory(new Category[] { new Category() }, "Regions").ContinueWith(t1 => LoadCities(t1.Result).ContinueWith(t => LoadChildrenFromCategory(t.Result, "Estatetypes").ContinueWith(t2 => LoadChildrenFromCategory(t2.Result, "Tags"))));

            // Load cities + tags
            await LoadChildrenFromCategory(new Category[] { new Category() }, "Regions").ContinueWith(t1 => LoadCities(t1.Result).ContinueWith(t => LoadChildrenFromCategory(t.Result, "Tags")));

            // Load cities + estate type + conditions
            await LoadChildrenFromCategory(new Category[] { new Category() }, "Regions").ContinueWith(t1 => LoadCities(t1.Result).ContinueWith(t => LoadChildrenFromCategory(t.Result, "Estatetypes").ContinueWith(t2 => LoadChildrenFromCategory(t2.Result, "Conditions"))));

            // Step 2. Load Estatetypes
            await LoadChildrenFromCategory(new Category[] { new Category() }, "Estatetypes");

            // Step 3. Load Tags
            await LoadChildrenFromCategory(new Category[] { new Category() }, "Tags");

            // Step 4. Load Conditions
            await LoadChildrenFromCategory(new Category[] { new Category() }, "Conditions");
            // Step 5. Load Other type
            await LoadChildrenFromCategory(new Category[] { new Category() }, "OtherType");
            _lockObject.Release();
            return _loadedCategory;
        }

        private async Task<Category[]> LoadChildrenFromCategory(Category[] parents, string productType)
        {
            var result = await _searchApi.SearchApiModule.SearchProductsAsync(_storeId, 
                new AutoRestClients.SearchApiModuleApi.Models.ProductSearch
                {
                    ResponseGroup = "1619",
                    Outline = ProductTypeToOutline(productType),
                    Take = int.MaxValue,
                    Skip = 0
                });

            foreach (var parent in parents)
            {
                var categories = new List<Category>();
                if (result.Products == null)
                {
                    break;
                }
                var children = result.Products.Select(p => ConvertProductToCategory(parent, productType, p));
                if (parent.Categories != null)
                {
                    categories.AddRange(parent.Categories);
                }
                categories.AddRange(children);

                parent.Categories = new MutablePagedList<Category>(categories);
            }

            return parents.SelectMany( p => p.Categories).ToArray();
        }

        private async Task<Category[]> LoadCities(Category[] parents)
        {
            var resultCities = await _searchApi.SearchApiModule.SearchProductsAsync(_storeId,
                new AutoRestClients.SearchApiModuleApi.Models.ProductSearch
                {
                    ResponseGroup = (ItemResponseGroup.ItemAssociations | ItemResponseGroup.Seo | ItemResponseGroup.ItemEditorialReviews | ItemResponseGroup.ItemInfo).ToString(),
                    Outline = ProductTypeToOutline("Cities"),
                    Take = int.MaxValue,
                    Skip = 0
                });
            foreach (var parent in parents)
            {
                var categories = new List<Category>();

                var children = resultCities.Products.Where(x => x.Associations.Any(a => a.AssociatedObjectId == parent.Id)).Select(p => ConvertProductToCategory(parent, "Cities", p));
                categories.AddRange(children);
                parent.Categories = new MutablePagedList<Category>(categories);
            }
            return parents.SelectMany(x => x.Categories).ToArray();
        }

        private string ProductTypeToOutline(string productType)
        {
            switch(productType)
            {
                case "Regions":
                    return ConfigurationManager.AppSettings["RegionCategoryId"];
                case "Estatetypes":
                    return ConfigurationManager.AppSettings["EstateTypeCategoryId"];
                case "Tags":
                    return ConfigurationManager.AppSettings["TagCategoryId"];
                case "Cities":
                    return ConfigurationManager.AppSettings["CityCategoryId"];
                case "Conditions":
                    return ConfigurationManager.AppSettings["ConditionCategoryId"];
                case "OtherType":
                    return ConfigurationManager.AppSettings["OtherTypeCategoryId"];
            }
            return string.Empty;
        }

        private Category ConvertProductToCategory(Category parent, string productType, AutoRestClients.SearchApiModuleApi.Models.Product dtoProduct)
        {
            var path = string.Join("/", parent.Path, productType);

            // TODO: Find Converter By Seo Path
            var converter = GetConverterByPath(path);

            var product = dtoProduct.ToProduct(_language, _currency, _store);

            var category = converter.ToCategory(new ConverterContext
                {
                    Parent = parent,
                    Path = path,
                    ProductType = productType
                }, product);
            var seoPath = category.SeoPath.Trim('/');
            if (!_seoCategoryDict.ContainsKey(seoPath))
            {
                _seoCategoryDict.Add(seoPath, category);
            }
            return category;
        }
        
        private ICategoryTreeConverter GetConverterByPath(string path)
        {
            if (path.EndsWith("Tag"))
            {
                return new TagCategoryTreeConverter();
            }
            else if (path.EndsWith("Estatetypes"))
            {
                return new TypeCategoryTreeConverter();
            }
            else if (path.EndsWith("Cities"))
            {
                return new CityCategoryTreeConverter();
            }
            else if (path.EndsWith("Regions"))
            {
                return new RegionCategoryTreeConverter();
            }
            else if (path.EndsWith("OtherType"))
            {
                return new OtherTypeCategoryTreeConverter();
            }
            else if (path.EndsWith("Conditions"))
            {
                return new ConditionCategoryTreeConverter();
            }
            return new DefaultCategoryTreeConverter();
        }

        public Category FindByPath(string path)
        {
            return _seoCategoryDict.ContainsKey(path) ? _seoCategoryDict[path] : null;
        }
    }
}