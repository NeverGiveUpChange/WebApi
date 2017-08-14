using HangFire_ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HangFire_Common
{
    public class CommonHelper
    {
        /// <summary>
        /// 生成返回数据模型
        /// </summary>
        /// <param name="statusCode">状态码</param>
        /// <param name="message">说明</param>
        /// <param name="data">返回数据</param>
        /// <returns></returns>
        public static Vm_ResponseData CreateResponseData(int statusCode, string message, object data = null)
        {
            return new Vm_ResponseData() { StatusCode = statusCode, Message = message, Data = data };
        }
     
        /// <summary>
        /// 生成批量插入sql语句
        /// </summary>
        /// <typeparam name="T">插入的数据类型</typeparam>
        /// <param name="source">插入的数据源</param>
        /// <param name="tableName">插入的表名</param>
        /// <returns></returns>
        public static string CreateBulkInsertSql<T>(IEnumerable<T> source, string tableName)
        {
            if (!source.Any()) throw new Exception("无数据");
            if (string.IsNullOrWhiteSpace(tableName)) throw new Exception("请设置插入的表名");
            StringBuilder sb = new StringBuilder();
            sb.Append("Insert into " + tableName + "(");
            Type t = typeof(T);
            foreach (var item in t.GetProperties())
            {
                sb.Append(item.Name + ",");
            }
            sb.Remove(sb.ToString().LastIndexOf(','), 1);
            sb.Append(") Values ");
            foreach (var item in source)
            {
                Type type = item.GetType();
                sb.Append("(");
                foreach (var pi in type.GetProperties())
                {
                    sb.Append("'" + type.GetProperty(pi.Name).GetValue(item, null) + "',");
                }
                sb.Remove(sb.ToString().LastIndexOf(','), 1);
                sb.Append("),");
            }
            sb.Remove(sb.ToString().LastIndexOf(','), 1);
            sb.Append(";");
            sb.Append("select @@IDENTITY");
            return sb.ToString();
        }
        public static string RSADecrypt(string privateKey, string content)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(privateKey);
            cipherbytes = rsa.Decrypt(Convert.FromBase64String(content), false);
            return HttpUtility.UrlDecode( Encoding.UTF8.GetString(cipherbytes));

        }

    }
}
