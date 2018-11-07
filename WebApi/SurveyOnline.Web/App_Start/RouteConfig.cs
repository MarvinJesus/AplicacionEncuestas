using System.Web.Mvc;
using System.Web.Routing;

namespace SurveyOnline.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Topic",
                url: "Topic",
                defaults: new { controller = "Topic", action = "Topic" }
                );
            routes.MapRoute(
                name: "Logout",
                url: "Logout",
                defaults: new { controller = "Home", action = "Logout" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
