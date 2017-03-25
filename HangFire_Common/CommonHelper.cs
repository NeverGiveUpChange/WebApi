using HangFire_ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// 创建密钥
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="userName">用户姓名</param>
        /// <returns></returns>
        public static string CreateTicket(int userId, string userName)
        {
            return (userId + userName + DateTime.Now.ToString() + Guid.NewGuid().ToString()).ToMD5Hash();
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
        /// <summary>
        /// 生成批量更新的sql语句
        /// </summary>
        /// <typeparam name="T">更新的数据类型</typeparam>
        /// <param name="source">更新的数据源</param>
        /// <param name="tableName">更新的表名</param>
        /// <param name="updateByFiledName">更新依据的字段</param>
        /// <returns></returns>
        public static string CreateBulkUpdateSql<T>(IEnumerable<T> source, string tableName, string updateByFiledName)
        {
            if (!source.Any()) throw new Exception("无数据");
            if (string.IsNullOrWhiteSpace(tableName)) throw new Exception("请设置更新的表名");
            if (string.IsNullOrWhiteSpace(updateByFiledName)) throw new Exception("请设置依据的列");
            var sbUpdateSql = new StringBuilder("Update " + tableName + " set");
            var sbWhereSql = new StringBuilder(" Where " + updateByFiledName + " In(");
            var t = typeof(T);
            var properties = t.GetProperties().ToList();
            var updateByFiledIndex = -1;
            var index = -1;
            for (var i = 0; i < properties.Count; i++)
            {
                if (properties[i].Name != updateByFiledName) continue;
                updateByFiledIndex = i;
                break;
            }
            properties.RemoveAt(updateByFiledIndex);
            foreach (var propertie in properties)
            {
                index++;
                sbUpdateSql.Append(" " + propertie.Name + "=case " + updateByFiledName + " ");

                foreach (var item in source)
                {
                    if (index == 0)
                    {
                        sbWhereSql.Append("'" +
                                                  item.GetType().GetProperty(updateByFiledName).GetValue(item, null) + "',");
                    }
                    sbUpdateSql.Append(" when " + item.GetType().GetProperty(updateByFiledName).GetValue(item, null) + " Then '" + item.GetType().GetProperty(propertie.Name).GetValue(item, null) + "'");
                }
                sbUpdateSql.Append(" End ,");
            }
            return sbUpdateSql.ToString().Substring(0, sbUpdateSql.ToString().Length - 1) + " " +
                   sbWhereSql.ToString().Substring(0, sbWhereSql.ToString().Length - 1) + ")";
        }
       
    }
}
