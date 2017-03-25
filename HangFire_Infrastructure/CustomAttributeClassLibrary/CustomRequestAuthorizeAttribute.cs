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
  
    public class CustomRequestAuthorizeAttribute : ActionFilterAttribute
    {
        const string ticketName = "ticket";
        const int expiry = 300;
        CacheClient _cacheClient = new CacheClient(new RedisStringCache(0));
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
         
            var attributes = filterContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
            bool isAnonymous = attributes.Any(a => a is AllowAnonymousAttribute);
            if (!isAnonymous)
            {
                string ticketValue = GetTicket(filterContext);
                var response = new HttpResponseMessage();
                var authInfo = _cacheClient.Get<AuthorizeInfo>(ticketValue);
                if (authInfo == null || string.IsNullOrWhiteSpace(ticketValue))
                {
                    response.StatusCode = HttpStatusCode.Unauthorized;             
                    response.Content = new StringContent(CommonHelper.CreateResponseData(-10003, "请求不合法").ConvertJson());
                    filterContext.Response = response;
                }
                else
                {
                    _cacheClient.KeyExpire(ticketValue, TimeSpan.FromMinutes(expiry));
                }
            }

            base.OnActionExecuting(filterContext);
        }
        private string GetTicket(HttpActionContext context)
        {
            var ticket = string.Empty;
            var requestMethod = context.Request.Method.ToString();
            if (requestMethod == "get".ToUpper())
            {
                var qs = HttpUtility.ParseQueryString(context.Request.RequestUri.Query);
                ticket = qs[ticketName];
                ticket = ticket ?? qs["Ticket"];
            }
            else if (requestMethod == "post".ToUpper())
            {
                var stream = context.Request.Content.ReadAsStreamAsync();
                using (var sm = stream.Result)
                {
                    if (sm != null)
                    {
                        sm.Seek(0, SeekOrigin.Begin);
                        int len = (int)sm.Length;
                        byte[] inputByts = new byte[len];
                        sm.Read(inputByts, 0, len);
                        sm.Close();
                        var content = Encoding.UTF8.GetString(inputByts).ConvertObj<dynamic>();
                        ticket = content.Ticket == null ? content.ticket == null ? null : content.ticket.ToString() : content.Ticket.ToString();
                    }
                }
            }
            return ticket;
        }
    }
}
