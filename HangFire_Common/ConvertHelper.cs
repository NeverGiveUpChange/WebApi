using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFire_Common
{
    public static class ConvertHelper
    {
        #region 数值转换

        /// <summary>
        /// 转换为整型
        /// </summary>
        /// <param name="data">数据</param>
        public static int ToInt(object data)
        {
            if (data == null)
                return 0;
            int result;
            var success = int.TryParse(data.ToString(), out result);
            if (success)
                return result;
            try
            {
                return Convert.ToInt32(ToDouble(data, 0));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// 转换为可空整型
        /// </summary>
        /// <param name="data">数据</param>
        public static int? ToIntOrNull(object data)
        {
            if (data == null)
                return null;
            int result;
            var success = int.TryParse(data.ToString(), out result);
            if (success)
                return result;
            return null;
        }

        /// <summary>
        /// 转换为双精度浮点数
        /// </summary>
        /// <param name="data">数据</param>
        public static double ToDouble(object data)
        {
            if (data == null)
                return 0.00;
            double result;
            return double.TryParse(data.ToString(), out result) ? result : 0.00;
        }

        /// <summary>
        /// 转换为双精度浮点数,并按指定的小数位4舍5入
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="digits">小数位数</param>
        public static double ToDouble(object data, int digits)
        {
            return Math.Round(ToDouble(data), digits);
        }

        /// <summary>
        /// 转换为可空双精度浮点数
        /// </summary>
        /// <param name="data">数据</param>
        public static double? ToDoubleOrNull(object data)
        {
            if (data == null)
                return null;
            double result;
            var success = double.TryParse(data.ToString(), out result);
            if (success)
                return result;
            return null;
        }

        /// <summary>
        /// 转换为高精度浮点数
        /// </summary>
        /// <param name="data">数据</param>
        public static decimal ToDecimal(object data)
        {
            if (data == null)
                return 0m;
            decimal result;
            return decimal.TryParse(data.ToString(), out result) ? result : 0m;
        }

        /// <summary>
        /// 转换为高精度浮点数,并按指定的小数位4舍5入
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="digits">小数位数</param>
        public static decimal ToDecimal(object data, int digits)
        {
            return Math.Round(ToDecimal(data), digits);
        }

        /// <summary>
        /// 转换为可空高精度浮点数
        /// </summary>
        /// <param name="data">数据</param>
        public static decimal? ToDecimalOrNull(object data)
        {
            if (data == null)
                return null;
            decimal result;
            var success = decimal.TryParse(data.ToString(), out result);
            if (success)
                return result;
            return null;
        }

        /// <summary>
        /// 转换为可空高精度浮点数,并按指定的小数位4舍5入
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="digits">小数位数</param>
        public static decimal? ToDecimalOrNull(object data, int digits)
        {
            var result = ToDecimalOrNull(data);
            if (result == null)
                return null;
            return Math.Round(result.Value, digits);
        }

        #endregion

        #region Guid转换

        /// <summary>
        /// 转换为Guid
        /// </summary>
        /// <param name="data">数据</param>
        public static Guid ToGuid(object data)
        {
            if (data == null)
                return Guid.Empty;
            Guid result;
            return Guid.TryParse(data.ToString(), out result) ? result : Guid.Empty;
        }

        /// <summary>
        /// 转换为可空Guid
        /// </summary>
        /// <param name="data">数据</param>
        public static Guid? ToGuidOrNull(object data)
        {
            if (data == null)
                return null;
            Guid result;
            var success = Guid.TryParse(data.ToString(), out result);
            if (success)
                return result;
            return null;
        }

        /// <summary>
        /// 转换为Guid集合
        /// </summary>
        /// <param name="guid">guid集合字符串，范例:83B0233C-A24F-49FD-8083-1337209EBC9A,EAB523C6-2FE7-47BE-89D5-C6D440C3033A</param>
        public static List<Guid> ToGuidList(string guid)
        {
            var listGuid = new List<Guid>();
            if (string.IsNullOrWhiteSpace(guid))
                return listGuid;
            var arrayGuid = guid.Split(',');
            listGuid.AddRange(from each in arrayGuid where !string.IsNullOrWhiteSpace(each) select new Guid(each));
            return listGuid;
        }

        #endregion

        #region 日期转换

        /// <summary>
        /// 转换为日期
        /// </summary>
        /// <param name="data">数据</param>
        public static DateTime ToDate(object data)
        {
            if (data == null)
                return DateTime.MinValue;
            DateTime result;
            return DateTime.TryParse(data.ToString(), out result) ? result : DateTime.MinValue;
        }

        /// <summary>
        /// 转换为可空日期
        /// </summary>
        /// <param name="data">数据</param>
        public static DateTime? ToDateOrNull(object data)
        {
            if (data == null)
                return null;
            DateTime result;
            var isValid = DateTime.TryParse(data.ToString(), out result);
            if (isValid)
                return result;
            return null;
        }
        public static long ConvertToTimeStmap(DateTime dt)
        {
            return (dt.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        #endregion

        #region 布尔转换

        /// <summary>
        /// 转换为布尔值
        /// </summary>
        /// <param name="data">数据</param>
        public static bool ToBool(object data)
        {
            if (data == null)
                return false;
            var value = GetBool(data);
            if (value != null)
                return value.Value;
            bool result;
            return bool.TryParse(data.ToString(), out result) && result;
        }

        /// <summary>
        /// 获取布尔值
        /// </summary>
        private static bool? GetBool(object data)
        {
            switch (data.ToString().Trim().ToLower())
            {
                case "0":
                    return false;
                case "1":
                    return true;
                case "是":
                    return true;
                case "否":
                    return false;
                case "yes":
                    return true;
                case "no":
                    return false;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 转换为可空布尔值
        /// </summary>
        /// <param name="data">数据</param>
        public static bool? ToBoolOrNull(object data)
        {

            if (data == null)
                return null;
            var value = GetBool(data);
            if (value != null)
                return value.Value;
            bool result;
            var success = bool.TryParse(data.ToString(), out result);
            if (success)
                return result;
            return null;
        }

        #endregion

        #region 字符串转换

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <param name="data">数据</param>
        public static string ToString(object data)
        {
            return data == null ? string.Empty : data.ToString().Trim();
        }

        #endregion

        #region 通用转换

        /// <summary>
        /// 泛型转换
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="data">数据</param>
        public static T To<T>(object data)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.ToString()))
                return default(T);
            var type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            try
            {
                if (type.Name.ToLower() == "guid")
                    return (T)(object)new Guid(data.ToString());
                if (data is IConvertible)
                    return (T)Convert.ChangeType(data, type);
                return (T)data;
            }
            catch
            {
                return default(T);
            }
        }
        #endregion
    }
}
