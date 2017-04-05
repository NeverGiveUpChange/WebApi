using HangFire_Common;
using HangFire_Infrastructure.CacheHelper;
using HangFire_Infrastructure.CacheHelper.RedisCacheHelper;
using HangFire_ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace HangFire_Infrastructure.CustomAttributeClassLibrary
{

    public class CustomRequestAuthorizeAttribute : AuthorizeAttribute
    {

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var authorization = actionContext.Request.Headers.Authorization;
            if (authorization != null && !string.IsNullOrWhiteSpace(authorization.Parameter))
            {
                //请求头验证中添加  timestamp，key，sign
                AuthorizationInfo authorizationInfo = null;
                try
                {
                    authorizationInfo = GetAuthorizationInfo(authorization.Parameter);
                    if (authorizationInfo == null) {
                        HandleUnauthorizedRequest(actionContext);
                    }
                }
                catch
                {
                    HandleUnauthorizedRequest(actionContext);

                }
                //根据key获取secret
                var secret = "test";
                // 检查时间是否过期
                if (ConvertHelper.ConvertToTimeStmap(DateTime.Now) - long.Parse(authorizationInfo.TimeStamp) > 120)
                {
                    HandleUnauthorizedRequest(actionContext);
                }
                if ((secret + authorizationInfo.TimeStamp + authorizationInfo.Key).ToMD5Hash() != authorizationInfo.Sign)
                {
                    HandleUnauthorizedRequest(actionContext);
                }
                else {
                    base.OnAuthorization(actionContext);
                }
                // 使用统一的签名方式进行签名比较生成的sign 是否一样
            }
            else
            {
                var attributes = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
                var isAnonymous = attributes.Any(a => a is AllowAnonymousAttribute);
                if (isAnonymous) base.OnAuthorization(actionContext);
                else HandleUnauthorizedRequest(actionContext);
            }

        }
        private AuthorizationInfo GetAuthorizationInfo(string parameter)
        {
            var parameterArray = parameter.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            if (parameterArray.Length < 3 || string.IsNullOrWhiteSpace(parameterArray[0]) || string.IsNullOrWhiteSpace(parameterArray[1]) || string.IsNullOrWhiteSpace(parameterArray[2])) return null;
            else
            {
                parameter = string.Format("{0}{1}{2}", "{", parameter.Replace(" ", ","), "}");
                return parameter.ConvertObj<AuthorizationInfo>();
            }

        }
    }
    class AuthorizationInfo
    {
        public string TimeStamp { get; set; }
        public string Key { get; set; }
        public string Sign { get; set; }
    }


}
