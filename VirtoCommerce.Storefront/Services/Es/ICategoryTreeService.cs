﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using VirtoCommerce.Storefront.Model.Catalog;

namespace VirtoCommerce.Storefront.Services.Es
{
    public interface ICategoryTreeService
    {
        Category FindByPath(string path);
        Task<Dictionary<string, Category>> GetTree();
        Task<Dictionary<string, Category>> RebuildTree();
        Dictionary<string, Category> GetSeoDict();
        Task ClearTree();
        Task RebuildElement(string path);
    }
}