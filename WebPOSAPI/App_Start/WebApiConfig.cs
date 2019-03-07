using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebPOSAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // config.EnableCors(new EnableCorsAttribute("http://localhost:4200", headers: "*", methods:"*"));
            // config.EnableCors(new EnableCorsAttribute(origins: "*", headers: "*", methods:"*"));
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            // ReferenceLoopHandling.Ignore will solve the Self referencing loop detected error
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                //routeTemplate: "api/{controller}/{id}",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
