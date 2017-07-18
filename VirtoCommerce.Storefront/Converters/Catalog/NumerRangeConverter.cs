﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Storefront.Converters.Catalog
{
    public static class NumerRangeConverter
    {
        public static LiquidThemeEngine.Objects.NumericRange ToStorefrontModel(this Model.Common.NumericRange range)
        {
            if (range == null)
            {
                return null;
            }
            return new LiquidThemeEngine.Objects.NumericRange
            {
                IncludeLower = range.IncludeLower,
                IncludeUpper = range.IncludeUpper,
                Lower = range.Lower,
                Upper = range.Upper
            };
        }
    }
}