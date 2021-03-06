﻿namespace VirtoCommerce.Storefront.Model.Common
{
    public class NumericRange
    {
        public decimal? Lower { get; set; }
        public decimal? Upper { get; set; }
        public bool IncludeLower { get; set; }
        public bool IncludeUpper { get; set; }

        public override string ToString()
        {
            return $"{(IncludeLower ? "[" : "(")}{(Lower.HasValue ? Lower.ToString() : "")} TO {(Upper.HasValue ? Upper.ToString() : "")}{(IncludeUpper ? "]" : ")")}";
        }

        public override int GetHashCode()
        {
            return Lower.GetHashCode() + Upper.GetHashCode() + IncludeLower.GetHashCode() + IncludeUpper.GetHashCode();
        }
    }
}
