﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Storefront.Model.StaticContent
{
    /// <summary>
    /// TODO: Comments and author user info
    /// </summary>
    public partial class BlogArticle : ContentItem
    {
        private static string _excerpToken = "<!--excerpt-->";

        public override string Type { get { return "post"; } }

        public string Excerpt { get; set; }

        public string BlogName { get; set; }

        public string ImageUrl { get; set; }

        public bool IsSticked { get; set; }

        public bool IsTrending { get; set; }

        public IList<string> SliderImages { set; get; } = new List<string>();
        public bool ShowInMarketBlock { get; set; }

        public override void LoadContent(string content, IDictionary<string, IEnumerable<string>> metaInfoMap)
        {
            var parts = content.Split(new[] { _excerpToken }, StringSplitOptions.None);

            if (parts.Length > 1)
            {
                Excerpt = parts[0];
                content.Replace(_excerpToken, string.Empty);
            }

            if (metaInfoMap.ContainsKey("main-image"))
            {
                ImageUrl = metaInfoMap["main-image"].FirstOrDefault();
            }

            if (metaInfoMap.ContainsKey("excerpt"))
            {
                Excerpt = metaInfoMap["excerpt"].FirstOrDefault();
            }

            if (metaInfoMap.ContainsKey("is-sticked"))
            {
                var isSticked = false;

                bool.TryParse(metaInfoMap["is-sticked"].FirstOrDefault(), out isSticked);

                IsSticked = isSticked;
            }

            if (metaInfoMap.ContainsKey("is-trending"))
            {
                var isTrending = false;

                bool.TryParse(metaInfoMap["is-trending"].FirstOrDefault(), out isTrending);

                IsTrending = isTrending;
            }

            if (metaInfoMap.ContainsKey("slider-images"))
            {
                SliderImages = metaInfoMap["slider-images"].ToList();
            }
            if (metaInfoMap.ContainsKey("showInMarketBlock"))
            {
                var isShowInMarketBlock = false;
                bool.TryParse(metaInfoMap["showInMarketBlock"].FirstOrDefault(), out isShowInMarketBlock);
                ShowInMarketBlock = isShowInMarketBlock;
            }

            if (metaInfoMap.ContainsKey("meta-description"))
            {
                MetaDescription = metaInfoMap["meta-description"].FirstOrDefault();
            }

            if (metaInfoMap.ContainsKey("page-title"))
            {
                PageTitle = metaInfoMap["page-title"].FirstOrDefault();
            }
            base.LoadContent(content, metaInfoMap);
        }
    }
}
