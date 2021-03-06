﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Storefront.Model.Catalog;

namespace VirtoCommerce.Storefront.Services.Es.Converters
{
    public class SinglePageCategoryTreeConverter : DefaultCategoryTreeConverter
    {
        public override Category ToCategory(ConverterContext context, Product product)
        {
            var category = base.ToCategory(context, product);
            category.SeoPath = product.SeoInfo?.Slug;
            category.Url = product.SeoInfo?.Slug;
            category.Type = "city";
            return category;
        }

        protected override string CreateFullName(ConverterContext context, Product product)
        {
            return product.Name;
        }
    }
}