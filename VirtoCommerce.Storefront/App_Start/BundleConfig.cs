using System.Web.Optimization;

namespace VirtoCommerce.Storefront
{
    public class BundleConfig
    {
        public bool Minify { get; set; }
        public IItemTransform[] CssItemTransforms { get; set; } = { new CssUrlTransform() };

        public virtual void RegisterBundles(BundleCollection bundles)
        { 
            #region JS

            bundles.Add(
                CreateScriptBundle("~/default-theme/scripts")
                    .Include("~/App_Data/Themes/default/assets/modernizr.min.js")
                    .Include("~/App_Data/Themes/default/assets/interactor.js")
                    .Include("~/App_Data/Themes/default/assets/ideal-image-slider.min.js")
                    .Include("~/App_Data/Themes/default/assets/ideal-image-slider-bullet-nav.js")
                    .Include("~/App_Data/Themes/default/assets/ideal-image-slider-captions.js")
                    .IncludeDirectory("~/App_Data/Themes/default/assets/js/", "*.js"));

            bundles.Add(
                CreateScriptBundle("~/default-theme/checkout/scripts")
                    .Include("~/App_Data/Themes/default/assets/js/app.js")
                    .Include("~/App_Data/Themes/default/assets/js/services.js")
                    .Include("~/App_Data/Themes/default/assets/js/directives.js")
                    .Include("~/App_Data/Themes/default/assets/js/main.js")
                    .IncludeDirectory("~/App_Data/Themes/default/assets/js/common-components/", "*.js")
                    .IncludeDirectory("~/App_Data/Themes/default/assets/js/checkout/", "*.js"));

            bundles.Add(
               CreateScriptBundle("~/default-theme/account/scripts")
                    .Include("~/App_Data/Themes/default/assets/modernizr.min.js")
                    .Include("~/App_Data/Themes/default/assets/js/app.js")
                    .Include("~/App_Data/Themes/default/assets/js/services.js")
                    .Include("~/App_Data/Themes/default/assets/js/main.js")
                    .Include("~/App_Data/Themes/default/assets/js/cart.js")
                    .Include("~/App_Data/Themes/default/assets/js/quote-request.js")
                    .Include("~/App_Data/Themes/default/assets/js/product-compare.js")
                    .IncludeDirectory("~/App_Data/Themes/default/assets/js/common-components/", "*.js")
                    .IncludeDirectory("~/App_Data/Themes/default/assets/js/account/", "*.js"));
            bundles.Add(
                CreateScriptBundle("~/theme-bundler/scripts")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/bootstrap.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/modernizr.custom.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/jquery.ui.core.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/jquery.lazyload.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/jquery.nicescroll.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/dense.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/encoder.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/infobox.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/owl.carousel.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/placeholders.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/property.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/slick.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/widget.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/mouse.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/menu.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/draggable.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/position.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/custom-js.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/control.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/slider.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/jquery.contentcarousel.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/slider-custom.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/jquery.mousewheel.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/filter.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/loader.js"));

            bundles.Add(CreateScriptBundle("~/theme-bundler/scripts/map")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/map/google_map_code_listing.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/map/infobox.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/map/mapfunctions.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/map/markerclusterer.js")
                    .Include("~/App_Data/Themes/es/assets/static/js/theme-js/map/oms.js"));
            #endregion

            #region CSS

            bundles.Add(new StyleBundle("~/es-theme/css-base")
                .Include("~/App_Data/Themes/es/assets/static/styles/bootstrap-theme.css")
                .Include("~/App_Data/Themes/es/assets/static/styles/bootstrap.css")
                .Include("~/App_Data/Themes/es/assets/static/styles/style.css")
                .Include("~/App_Data/Themes/es/assets/static/styles/custom-style.css")
                .Include("~/App_Data/Themes/es/assets/static/styles/static.css")
                .Include("~/App_Data/Themes/es/assets/static/styles/mobile.css")
                .Include("~/App_Data/Themes/es/assets/static/styles/my_media.css")
                .Include("~/App_Data/Themes/es/assets/static/styles/orange-theme.css")
            );
            bundles.Add(CreateStyleBundle("~/es-theme/tilda")
                .Include("~/App_Data/Themes/es/assets/static/styles/tilda-blocks-2.12.css", CssItemTransforms)
                .Include("~/App_Data/Themes/es/assets/static/styles/tilda-grid-3.0.css", CssItemTransforms)
                .Include("~/App_Data/Themes/es/assets/static/styles/tilda-zoom-1.0.css", CssItemTransforms)
                .Include("~/App_Data/Themes/es/assets/static/styles/tooltipster.css", CssItemTransforms)
            );
            bundles.Add(
                CreateStyleBundle("~/default-theme/css")
                    .Include("~/App_Data/Themes/default/assets/storefront.css", CssItemTransforms)
                    .Include("~/App_Data/Themes/default/assets/common-components.css", CssItemTransforms)
                    .Include("~/App_Data/Themes/default/assets/ideal-image-slider.css", CssItemTransforms)
                    .Include("~/App_Data/Themes/default/assets/ideal-image-slider-default-theme.css", CssItemTransforms));

            bundles.Add(
                new StyleBundle("~/default-theme/account/css")
                .Include("~/App_Data/Themes/default/assets/account-bootstrap.css", CssItemTransforms)
                .Include("~/App_Data/Themes/default/assets/common-components.css", CssItemTransforms));
            
            #endregion
        }


        protected virtual ScriptBundle CreateScriptBundle(string virtualPath)
        {
            var bundle = new ScriptBundle(virtualPath);

            if (!Minify)
            {
                bundle.Transforms.Clear();
            }

            return bundle;
        }

        protected virtual StyleBundle CreateStyleBundle(string virtualPath)
        {
            var bundle = new StyleBundle(virtualPath);

            if (!Minify)
            {
                bundle.Transforms.Clear();
            }

            return bundle;
        }
    }
}
