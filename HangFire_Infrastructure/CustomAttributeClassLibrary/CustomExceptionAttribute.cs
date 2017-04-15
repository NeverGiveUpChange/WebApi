using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using HangFire_Common;
using HangFire_Infrastructure.CacheHelper.RedisCacheHelper;
using HangFire_Infrastructure.CacheHelper;
using System.Threading;
using System.Threading.Tasks;

namespace HangFire_Infrastructure.CustomAttributeClassLibrary
{
    public class CustomExceptionAttribute : ExceptionFilterAttribute
    {
        static CacheClient cacheClient = new CacheClient(new RedisListCache());
        const string key = "redisKey_exception";

        public override void OnException(HttpActionExecutedContext actionExecutedContext)//为什么出现异常会执行两次，是否是引发redis异常的原因
        {
            if (actionExecutedContext == null) { return; }
            EnqueueAsync(actionExecutedContext.Exception);//使用redisList当作队列记录日志
            var response = new HttpResponseMessage();
            if (actionExecutedContext.Exception is NotImplementedException)
            {
                response.StatusCode = HttpStatusCode.NotImplemented;
                response.Content = new StringContent(CommonHelper.CreateResponseData(-10000, "方法不被允许").ConvertJson());
            }
            else if (actionExecutedContext.Exception is TimeoutException)
            {
                response.StatusCode = HttpStatusCode.RequestTimeout;
                response.Content = new StringContent(CommonHelper.CreateResponseData(-10001, "请求超时").ConvertJson());
            }//其他错误类型统一返回500
            else
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Content = new StringContent(CommonHelper.CreateResponseData(-10002, "服务器内部错误").ConvertJson());
            }
            actionExecutedContext.Response = response;
            base.OnException(actionExecutedContext);

        }
        /// <summary>
        /// 入队
        /// </summary>
        private async  Task EnqueueAsync(Exception ex)
        {
          await  cacheClient.SetAsync<Exception>(key, ex);
        }
        /// <summary>
        /// 出队
        /// </summary>
        public static void Dequeue<T>(Action<T> action)
        {
            //默认使用线程池
            Task.Run(() =>
            {
                while (true)
                {
                    if (cacheClient.GetListLength(key) > 0)
                    {
                        var exception = cacheClient.Get<T>(key);
                        action(exception);
                    }
                    else
                    {
                        Thread.Sleep(2000);
                    }
                }
            });
            #region 线程池版本
            //ThreadPool.QueueUserWorkItem(o =>
            //{
            //    while (true)
            //    {
            //        if (cacheClient.GetListLength(key) > 0)
            //        {
            //            var exception = cacheClient.Get<T>(key);
            //            action(exception);
            //        }
            //        else
            //        {
            //            Thread.Sleep(2000);
            //        }
            //    }
            //});
            #endregion

        }

    }
}
