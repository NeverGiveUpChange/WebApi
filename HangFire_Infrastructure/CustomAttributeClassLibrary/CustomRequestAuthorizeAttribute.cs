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
            string method = request.Method.Method, timeStamp = string.Empty, expireyTime = ConfigurationManager.AppSettings["UrlExpireTime"], timeSign = string.Empty, platformType = string.Empty;
            if (!request.Headers.Contains("timesign") || !request.Headers.Contains("platformtype") || !request.Headers.Contains("timestamp") || !request.Headers.Contains("expiretime"))
            {
                HandleUnauthorizedRequest(actionContext);
                return;
            }
            platformType = request.Headers.GetValues("platformtype").FirstOrDefault();
            timeSign = request.Headers.GetValues("timesign").FirstOrDefault();
            timeStamp = request.Headers.GetValues("timestamp").FirstOrDefault();
            var tempExpireyTime = request.Headers.GetValues("expiretime").FirstOrDefault();
            string privateKey = Encoding.UTF8.GetString(Convert.FromBase64String(ConfigurationManager.AppSettings[$"PlatformPrivateKey_{platformType}"]));
            if (!SignValidate(tempExpireyTime, privateKey, timeStamp, timeSign))
            {
                HandleUnauthorizedRequest(actionContext);
                return;
            }
            if (tempExpireyTime != "0")
            {
                expireyTime = tempExpireyTime;
            }
            //判断timespan是否有效
            double ts2 = ConvertHelper.ToDouble((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds, 2), ts = ts2 - ConvertHelper.ToDouble(timeStamp);
            bool falg = ts > int.Parse(expireyTime) * 1000;
            if (falg)
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
        private bool SignValidate(string expiryTime, string privateKey, string timestamp, string sign)
        {
            bool isValidate = false;
            var tempSign = CommonHelper.RSADecrypt(privateKey, sign);
            if (CommonHelper.EncryptSHA256($"expiretime{expiryTime}" + $"timestamp{timestamp}") == tempSign)
            {
                isValidate = true;
            }
            return isValidate;
        }
    }

}
