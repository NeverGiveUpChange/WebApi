using HangFire_Infrastructure.CustomAttributeClassLibrary;
using System.Web;
using System.Web.Mvc;


namespace HangFireApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
           //筛选器运行顺序在mvc版本1，2是正向执行，版本3以后已反转
            filters.Add(new HandleErrorAttribute());
 
            
        }
    }
}
