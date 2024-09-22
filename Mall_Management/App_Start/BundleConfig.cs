using System.Web;
using System.Web.Optimization;

public class BundleConfig
{
    public static void RegisterBundles(BundleCollection bundles)
    {
        // Bundle cho CSS
        bundles.Add(new StyleBundle("~/Content/css").Include(
                  "~/Content/bootstrap.css",
                  "~/Content/custom.css"));

        // Bundle cho JavaScript
        bundles.Add(new ScriptBundle("~/bundles/customScript").Include(
                  "~/Scripts/script.js"));

        bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                  "~/Scripts/bootstrap.js"));

        BundleTable.EnableOptimizations = true;
    }
}
