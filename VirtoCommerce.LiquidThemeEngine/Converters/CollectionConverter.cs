﻿using System.Linq;
using Microsoft.Practices.ServiceLocation;
using PagedList;
using VirtoCommerce.LiquidThemeEngine.Filters;
using VirtoCommerce.LiquidThemeEngine.Objects;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using storefrontModel = VirtoCommerce.Storefront.Model.Catalog;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class CollectionStaticConverter
    {
        public static Collection ToShopifyModel(this storefrontModel.Category category, WorkContext workContext)
        {
            var converter = ServiceLocator.Current.GetInstance<ShopifyModelConverter>();
            return converter.ToLiquidCollection(category, workContext);
        }
    }

    public partial class ShopifyModelConverter
    {

        public virtual Collection ToLiquidCollection(storefrontModel.Category category, WorkContext workContext)
        {
            var result = new Collection();

            result.Id = category.Id;
            result.Description = null;
            result.Handle = category.SeoInfo != null ? category.SeoInfo.Slug : category.Id;
            result.Title = category.Name;
            result.Url = category.Url;
            result.DefaultSortBy = "manual";
            result.Images = category.Images.Select(x => x.ToShopifyModel()).ToArray();
            if (category.PrimaryImage != null)
            {
                result.Image = ToLiquidImage(category.PrimaryImage);
            }

            if (category.Products != null)
            {
                result.Products = new MutablePagedList<Product>((pageNumber, pageSize, sortInfos) =>
                {
                    // fill count products in collection for metho info
                    if (category.SeoInfo != null && category.SeoInfo.MetaDescription != null)
                    {
                        category.SeoInfo.MetaDescription = string.Format(category.SeoInfo.MetaDescription, StringFilters.NumEnding(category.Products.Count, "представлен", "представлено", "представлено", false), StringFilters.NumEnding(category.Products.Count, "объект", "объекта", "объектов"));
                    }
                    category.Products.Slice(pageNumber, pageSize, sortInfos);
                    return new StaticPagedList<Product>(category.Products.Select(x => ToLiquidProduct(x)), category.Products);
                }, category.Products.PageNumber, category.Products.PageSize);
            }

            if (category.Categories != null)
            {
                result.Collections = new Collections(new MutablePagedList<Collection>((pageNumber, pageSize, sortInfos) =>
                 {
                     category.Categories.Slice(pageNumber, pageSize, sortInfos);
                     return new StaticPagedList<Collection>(category.Categories.Select(x => ToLiquidCollection(x, workContext)), category.Categories);
                 }, category.Categories.PageNumber, category.Categories.PageSize));
            }

            if (workContext.Aggregations != null)
            {
                result.Tags = new TagCollection(new MutablePagedList<Tag>((pageNumber, pageSize, sortInfos) =>
                {
                    workContext.Aggregations.Slice(pageNumber, pageSize, sortInfos);
                    var tags = workContext.Aggregations.Where(a => a.Items != null)
                                           .SelectMany(a => a.Items.Select(item => ToLiquidTag(item, a.Field, a.Label)));
                    return new StaticPagedList<Tag>(tags, workContext.Aggregations);

                }, 1, int.MaxValue));
            }

            if (workContext.CurrentProductSearchCriteria.SortBy != null)
            {
                result.SortBy = workContext.CurrentProductSearchCriteria.SortBy;
            }

            if (!category.Properties.IsNullOrEmpty())
            {
                result.Metafields = new MetaFieldNamespacesCollection(new[] { new MetafieldsCollection("properties", category.Properties) });
            }

            return result;
        }
    }
}
