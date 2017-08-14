using HangFire_Common;
using System;
using System.Collections.Generic;
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
            var privateKey = @"<RSAKeyValue><Modulus>lfbv1KAPpNVI6b73tFoLzmWtTNPFePfie2RQRu9pjD4dntgT9Mq94Tg8409E7QgYs3c/BiD/gvKnmar8YHy2Li+BS9pYwVMcPfS2hRD7AOfO2n2bcHe3wHwtrpkePFjISEorqQ1K7AB0cgfPzSM3HUe72KPrXcVufsVaqKx4d9U=</Modulus><Exponent>AQAB</Exponent><P>xRJ0fK2YD8LnZedP8ngT8HcAvZ39+PnwNZSClDCunXLMoX8P6qjuBnQwl2kVSeoiGu17Q2PHmO/BWVS/xYaVWw==</P><Q>ws57BeW8ZohtRB8zU1LOYsEYCZPgMZw9oZvEUj69hNUTls/QTPFP48ve0eSIFj53q2IyHi+IXdLalvnhYY0+jw==</Q><DP>XLaB9uX0U/XtxxM7mWbEeqyBvLNHeIqdZ5emlvEQNSJ9Ar00T8GJuZ+P/KuDSHLN+L0g8GNQ07J30/76+YKHYQ==</DP><DQ>QfuaI9c6xyfyip5ezhxAF6b7s1LJH0Sig3i1OH5Txwg9CFn/XfBmdOv63vEfPxmNgxlRjHPhJpZiv7yHoHks9Q==</DQ><InverseQ>YwxNiov8FX0AChAmyWPVmrEZTYWmd3xGx6g8E7+Cfmj9OLi87YIVdxKcSCHQxP2ktTNK7XGLAqsZF1JRVSASpQ==</InverseQ><D>iJb+IsAW/590ZdKWcyKBFmDt08gfWOsO0iywIav2itIY0JEmvBPdlsFOVAvjT7HMLcozETYFBBRYK1LdxFRS6aFUIkeffmQxtAte/INBNNlJ5u5qgeLW7WZxmH3bU7IQirmaSyK3Zhi7ZgfRtAcJtSWwjEZFRW40iMpbxo3s+JE=</D></RSAKeyValue>";
            if (!request.Headers.Contains("platformid"))
            {
                return request;
            }
            //根据平台编号获得对应私钥
            //privateKey = request.Headers.GetValues("platformid").FirstOrDefault();

            if (request.Content.IsMimeMultipartContent())
                return request;
            // 读取请求body中的数据
            string baseContent = request.Content.ReadAsStringAsync().Result;
            string baseQuery = request.RequestUri.Query;
            if (!string.IsNullOrWhiteSpace(baseContent))
            {
                // 获取加密的信息
                // 兼容 body: 加密数据  和 body: code=加密数据
                baseContent =Regex.Match( baseContent, "(code=)*(?<code>[\\S]+)").Groups[2].Value;
                // 用加密对象解密数据
                baseContent = CommonHelper.RSADecrypt(Encoding.UTF8.GetString(Convert.FromBase64String(privateKey)), baseContent);
                // 将解密后的BODY数据 重置
                request.Content = new StringContent(baseContent);
            }
            if (string.IsNullOrWhiteSpace(baseQuery))
            {
                // 读取请求 url query数据
                baseQuery = baseQuery.Substring(1);
                baseQuery = Regex.Match(baseContent, "(code=)*(?<code>[\\S]+)").Groups[2].Value;
                baseQuery = baseContent = CommonHelper.RSADecrypt(Encoding.UTF8.GetString(Convert.FromBase64String(privateKey)), baseContent);
                // 将解密后的 URL 重置URL请求
                request.RequestUri = new Uri($"{request.RequestUri.AbsoluteUri.Split('?')[0]}?{baseQuery}");
            }
            //此contentType必须最后设置 否则会变成默认值
            request.Content.Headers.ContentType = contentType;

            return request;
        }
        protected override HttpResponseMessage ProcessResponse(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
