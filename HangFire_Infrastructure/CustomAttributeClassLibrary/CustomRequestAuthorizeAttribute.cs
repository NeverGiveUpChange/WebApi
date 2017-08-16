using HangFire_Common;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace HangFire_Infrastructure.CustomAttributeClassLibrary
{

    public class CustomRequestAuthorizeAttribute : AuthorizeAttribute
    {
        
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //action具有[AllowAnonymous]特性不参与验证
            if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>().Any(x => x is AllowAnonymousAttribute))
            {
                base.OnAuthorization(actionContext);
                return;
            }
            var request = actionContext.Request;
            string method = request.Method.Method;
            string timestamp = string.Empty;
            string expireTime = ConfigurationManager.AppSettings["UrlExpireTime"];
            if (request.Headers.Contains("timesign") && request.Headers.Contains("platformtype"))
            {
                //根据传过来的平台编号获得对应的私钥 解密得到对应的过期时间
                expireTime = CommonHelper.RSADecrypt(ConfigurationManager.AppSettings["PlatformPrivateKey_" + request.Headers.GetValues("platformtype").FirstOrDefault()], request.Headers.GetValues("timesign").FirstOrDefault()); ;
            }
            //根据请求类型拼接参数
            NameValueCollection form = HttpContext.Current.Request.QueryString;
            string data = string.Empty;
            switch (method)
            {
                case "POST":
                    
                    Stream stream = HttpContext.Current.Request.InputStream;
                    string responseJson = string.Empty;
                    StreamReader streamReader = new StreamReader(stream);
                    data = streamReader.ReadToEnd();
                    timestamp = GetTimeTamp(() => { return data.ConvertObj<dynamic>().timestamp; });
                    break;
                case "GET":
                    timestamp = GetTimeTamp(() => { return form.GetValues("timestamp").FirstOrDefault(); });
                    break;
                default:
                    HandleUnauthorizedRequest(actionContext);
                    return;
            }
            if (string.IsNullOrWhiteSpace(timestamp)) {
                HandleUnauthorizedRequest(actionContext);
                return;
            }
            //判断timespan是否有效
            double ts1 = 0;
            double ts2 = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds;
            bool timespanvalidate = double.TryParse(timestamp, out ts1);
            double ts = ts2 - ts1;
            bool falg = ts > int.Parse(expireTime) * 1000;
            if (falg || (!timespanvalidate))
            {
                HandleUnauthorizedRequest(actionContext);
                return;
            }
            base.IsAuthorized(actionContext);
        }
        protected override void HandleUnauthorizedRequest(HttpActionContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);

            var response = filterContext.Response = filterContext.Response ?? new HttpResponseMessage();
            response.StatusCode = HttpStatusCode.Forbidden;
            var content = new
            {
                BusinessStatus = -10403,
                StatusMessage = "服务端拒绝访问"
            };
            response.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        }
        private string GetTimeTamp(Func<dynamic> getTimesTampFunc)
        {
            try
            {
                return getTimesTampFunc();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }

        }
    }

}
