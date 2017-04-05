
using HangFire_Infrastructure.CustomAttributeClassLibrary;
using HangFire_Infrastructure.LogHelper;
using HangFireApi.App_Start;
using System;
using System.Data.Entity.Infrastructure.Interception;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HangFireApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoFacConfig.BuiderIocContainer();
            GlobalConfiguration.Configuration.Filters.Add(new CustomExceptionAttribute());
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(Server.MapPath("/Configs/log4net.config")));
            DbInterception.Add(new DataBaseLogger());
            CustomExceptionAttribute.Dequeue<Exception>(x => { Log.Error(x, Log.LogError); });

        }
    }
}
