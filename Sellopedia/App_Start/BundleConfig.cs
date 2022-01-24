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
