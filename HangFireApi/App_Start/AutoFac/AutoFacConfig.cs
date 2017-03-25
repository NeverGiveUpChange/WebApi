using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Mvc;
using HangFire_EFModel;

namespace HangFireApi.App_Start
{
    public class AutoFacConfig
    {
        public static void BuiderIocContainer()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.Register(o => new HangFire_DevEntities()).InstancePerRequest();//实现每个请求内ef上下文唯一
            builder.RegisterAssemblyTypes( Assembly.Load("HangFire_Service"))
            .Where(t => t.Name.EndsWith("Service") && !t.IsAbstract)//查找所有程序集下面以Service结尾的类  
            .AsImplementedInterfaces().InstancePerLifetimeScope(); //将找到的类和对应的接口放入IOC容器，并实现生命周期内唯一  
            builder.RegisterAssemblyTypes(Assembly.Load("HangFire_Repository"))//查找程序集中以Repository结尾的类型    
            .Where(t =>  t.Name.EndsWith( "DbSession"))
            .AsImplementedInterfaces().InstancePerRequest();//实现每个请求会话唯一
            builder.RegisterAssemblyTypes(Assembly.Load("HangFire_Repository")).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();//表示注册的类型，以接口的方式注册 ,并实现生命周期内唯一  
            var container = builder.Build(); //Build()方法是表示：创建一个容器  
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);//注册api容器需要使用HttpConfiguration对象  
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));//注册MVC容器  
        }
    }
}