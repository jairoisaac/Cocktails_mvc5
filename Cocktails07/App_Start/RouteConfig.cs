using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Cocktails07
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                "Delete Ingredient",
                "DeleteIngredient/{ckId}/{Tp}/{ingId}",
                new { controller = "Cocktail", action = "DeleteIngredient" }
            );
            routes.MapRoute(
                "Amount of Ingredient",
                "Amount/{ckId}/{Tp}/{Amt}/{ingId}",
                new { controller = "Cocktail", action = "EditIngredientAmount" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}