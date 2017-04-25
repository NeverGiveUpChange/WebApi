using HangFire_ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace HangFire_Common
{
  public static  class Extensions
    {
     
        public static byte[] StringToBytes(this string str,string encoding)
        {
           return Encoding.GetEncoding(encoding).GetBytes(str);
        }
        public static string BytesToString(this byte[] bytes, string encoding)
        {
            return Encoding.GetEncoding(encoding).GetString(bytes);
        }
      /// <summary>
      /// 序列化
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="value"></param>
      /// <returns></returns>
      public static string ConvertJson<T>(this T value)
      {
          string result = value is string ? value.ToString() : JsonConvert.SerializeObject(value);
          return result;
      }
      /// <summary>
      /// 反序列化
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="value"></param>
      /// <returns></returns>
      public static T ConvertObj<T>(this string value)
      {
         return  JsonConvert.DeserializeObject<T>(value);
      }
      /// <summary>
      /// 转换为HttpResponseMessage
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      public static HttpResponseMessage ToHttpResponseMessage(this Vm_ResponseData value)
      {
          return new HttpResponseMessage() { Content = new StringContent(value.ConvertJson()) };
      }
      /// <summary>
      /// MD5（byte[]）
      /// </summary>
      /// <param name="bytes"></param>
      /// <returns></returns>
      public static string ToMD5Hash(this byte[] bytes)
      {
          StringBuilder hash = new StringBuilder();
          MD5 md5 = MD5.Create();

          md5.ComputeHash(bytes)
                .ToList()
                .ForEach(b => hash.AppendFormat("{0:x2}", b));

          return hash.ToString();
      }
      /// <summary>
      /// MD5(string)
      /// </summary>
      /// <param name="inputString"></param>
      /// <returns></returns>
      public static string ToMD5Hash(this string inputString)
      {
          return Encoding.UTF8.GetBytes(inputString).ToMD5Hash();
      }
      /// <summary>
      /// 去重
      /// </summary>
      /// <typeparam name="TSource"></typeparam>
      /// <typeparam name="TKey"></typeparam>
      /// <param name="source"></param>
      /// <param name="keySelector"></param>
      /// <returns></returns>
      public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
      {
          HashSet<TKey> seenKeys = new HashSet<TKey>();
          foreach (TSource element in source)
          {
              if (seenKeys.Add(keySelector(element)))
              {
                  yield return element;
              }
          }
      }
    }
}
