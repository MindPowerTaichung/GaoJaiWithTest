using MPERP2015.MP.Log;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace MPERP2015
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            //MPExceptionLog
            config.Services.Add(typeof(IExceptionLogger), new MPExceptionLog());

            //移除XmlFormatter
            config.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);

            //JsonFormatter: camelCase in asp.net web api
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //JsonFormatter: 移除ReferenceLoopHandling
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            // Web API routes
            config.Routes.IgnoreRoute("Handler", "{whatever}.ashx/{*pathInfo}");

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
