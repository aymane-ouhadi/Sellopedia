using System.Web;
using System.Web.Optimization;

namespace Sellopedia
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            //--------------------- Bootstrap Templates & others--------------------//
            // --- User Template --- //
            bundles.Add(new StyleBundle("~/Content/userTemplate").Include(
                "~/Content/lib/owlcarousel/assets/owl.carousel.min.css",
                "~/Content/css/style.css"));

            bundles.Add(new ScriptBundle("~/bundles/userTemplate").Include(
                "~/Content/lib/easing/easing.min.js",
                "~/Content/lib/owlcarousel/owl.carousel.min.js",
                "~/Content/mail/jqBootstrapValidation.min.js",
                "~/Content/mail/contact.js",
                "~/Content/js/main.js"));

            // --- Admin Template --- //
            bundles.Add(new StyleBundle("~/Content/AdminTemplate").Include(
                "~/Content/AdminTemplate/plugins/bower_components/chartist/dist/chartist.min.css",
                "~/Content/AdminTemplate/plugins/bower_components/chartist-plugin-tooltips/dist/chartist-plugin-tooltip.css",
                "~/Content/AdminTemplate/css/style.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/adminTemplate").Include(
                "~/Content/AdminTemplate/plugins/bower_components/jquery/dist/jquery.min.js",
                "~/Content/AdminTemplate/bootstrap/dist/js/bootstrap.bundle.min.js",
                "~/Content/AdminTemplate/js/app-style-switcher.js",
                "~/Content/AdminTemplate/plugins/bower_components/jquery-sparkline/jquery.sparkline.min.js",
                "~/Content/AdminTemplate/js/waves.js",
                "~/Content/AdminTemplate/js/sidebarmenu.js",
                "~/Content/AdminTemplate/js/custom.js",
                "~/Content/AdminTemplate/plugins/bower_components/chartist/dist/chartist.min.js",
                "~/Content/AdminTemplate/plugins/bower_components/chartist-plugin-tooltips/dist/chartist-plugin-tooltip.min.js",
                "~/Content/AdminTemplate/js/pages/dashboards/dashboard1.js"));

            //------------------------ Custom css/js/jquery ------------------------//
            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                "~/Scripts/Custom/AutoComplete.js",
                "~/Scripts/Custom/StarsRating.js",
                "~/Scripts/Custom/PriceFilter.js",
                "~/Scripts/Custom/Cart.js"));

            bundles.Add(new StyleBundle("~/Content/custom").Include(
                "~/Content/Custom/Custom.css"));
        }
    }
}
