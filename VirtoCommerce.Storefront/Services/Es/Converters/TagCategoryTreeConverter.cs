﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Storefront.Model.Catalog;

namespace VirtoCommerce.Storefront.Services.Es.Converters
{
    public class TagCategoryTreeConverter: DefaultCategoryTreeConverter
    {
        public override Category ToCategory(ConverterContext context, Product product)
        {
            var category = base.ToCategory(context, product);
            category.Type = "tag";
            if (!string.IsNullOrEmpty(context.Parent.Type))
            {
                category.Type = "tag_add";
                // TODO: For not display info from main category
                category.Properties = new CatalogProperty[0];
                category.Description = string.Empty;
                category.Descriptions = new Model.EditorialReview[0];
                category.RegionUrl = context.Parent.RegionUrl;
                category.CityUrl = context.Parent.CityUrl;
                category.RegionName = context.Parent.RegionName;
                category.CityName = context.Parent.CityName;
            }
            else
            {
                category.Type = "tag";
            }
            return category;
        }
    }
}