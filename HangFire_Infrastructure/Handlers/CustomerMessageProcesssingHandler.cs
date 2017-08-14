using HangFire_Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;


namespace HangFire_Infrastructure.Handlers
{
    public class CustomerMessageProcesssingHandler : MessageProcessingHandler
    {
        protected override HttpRequestMessage ProcessRequest(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var contentType = request.Content.Headers.ContentType;

            if (!request.Headers.Contains("platformtype"))
            {
                return request;
            }
            //根据平台编号获得对应私钥
            string privateKey = Encoding.UTF8.GetString(Convert.FromBase64String(ConfigurationManager.AppSettings["PlatformPrivateKey_" + request.Headers.GetValues("platformtype").FirstOrDefault()]));

            if (request.Content.IsMimeMultipartContent())
                return request;
            // 读取请求body中的数据
            string baseContent = request.Content.ReadAsStringAsync().Result;
            string baseQuery = request.RequestUri.Query;
            if (!string.IsNullOrWhiteSpace(baseContent))
            {
                // 获取加密的信息
                // 兼容 body: 加密数据  和 body: code=加密数据
                baseContent = Regex.Match(baseContent, "(code=)*(?<code>[\\S]+)").Groups[2].Value;
                // 用加密对象解密数据
                baseContent = CommonHelper.RSADecrypt(privateKey, baseContent);
                // 将解密后的BODY数据 重置
                request.Content = new StringContent(baseContent);
            }
            if (!string.IsNullOrWhiteSpace(baseQuery))
            {
                // 读取请求 url query数据
                baseQuery = baseQuery.Substring(1);
                baseQuery = Regex.Match(baseContent, "(code=)*(?<code>[\\S]+)").Groups[2].Value;
                baseQuery = baseContent = CommonHelper.RSADecrypt(privateKey, baseContent);
                // 将解密后的 URL 重置URL请求
                request.RequestUri = new Uri($"{request.RequestUri.AbsoluteUri.Split('?')[0]}?{baseQuery}");
            }
            //此contentType必须最后设置 否则会变成默认值
            request.Content.Headers.ContentType = contentType;

            return request;
        }
        protected override HttpResponseMessage ProcessResponse(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            return response;
        }
    }
}
