using HangFire_Infrastructure.CustomAttributeClassLibrary;
using HangFire_Infrastructure.Handlers;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace HangFireApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.MessageHandlers.Add(new CustomerMessageProcesssingHandler());
            config.Filters.Add(new CustomExceptionAttribute());

   
        }
    }
}
