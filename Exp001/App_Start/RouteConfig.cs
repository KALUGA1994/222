﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Exp001
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "lang",
                url: "{lang}/{controller}/{action}/{id}",
                defaults: new { controller = "Getter", action = "Index", id = UrlParameter.Optional },
                constraints: new { lang = @"ru|en" },
                namespaces: new[] { "Exp001.Controllers" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Getter", action = "Index", id = UrlParameter.Optional, lang = "ru" },
                namespaces: new[] { "Exp001.Controllers" }
            );
   
        }
    }
}
