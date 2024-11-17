using System.Web.Mvc;
using System.Web.Routing;

public class RouteConfig
{
    public static void RegisterRoutes(RouteCollection routes)
    {
        routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
        );
        routes.MapRoute(
        name: "RentNow",
        url: "Home/RentNow",
        defaults: new { controller = "Home", action = "RentNow" }
        );
        routes.MapRoute(
            name: "ContractHistory",
            url: "home/history/{id}",
            defaults: new { controller = "Home", action = "History", id = UrlParameter.Optional }
        );
        routes.MapRoute(
        name: "Infor",
        url: "{Home}/{action}/{id}",
        defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
         );
    }

}
