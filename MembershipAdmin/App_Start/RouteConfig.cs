using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MembershipAdmin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "ApiRoute",
              url: "api/ajax/{action}",
              defaults: new { controller = "Ajax", action = "SetClanarinaAjax", id = UrlParameter.Optional }
            );
            //routes.MapRoute(
            //  name: "ApiRoute2",
            //  url: "api/ajax/{action}",
            //  defaults: new { controller = "Ajax", action = "GetClanarinaPayedcurrentMonthAjax", id = UrlParameter.Optional }
            //);
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
