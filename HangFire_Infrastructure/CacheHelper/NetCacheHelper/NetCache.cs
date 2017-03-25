using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace HangFire_Infrastructure.CacheHelper.NetCacheHelper
{
    public class NetCache:BaseCache
    {
        public static readonly string sysNetCacheKey = ConfigurationManager.AppSettings["NetCacheKey"] ?? "";
        public override T Get<T>(string key)
        {
            var value = HttpRuntime.Cache.Get(key);
            if (value == null) return default(T);
            if (value is string)
            {
                var resultValue = HttpRuntime.Cache.Get(key) as string;
                return CacheCommon.ConvertObj<T>(resultValue);
            }
            else
            {
                return (T)value;
            }
        }
        public override bool Set<T>(string key, T value, TimeSpan? expiry = default(TimeSpan?))
        {
            var newValue = CacheCommon.ConvertJson<T>(value);
            if (expiry.HasValue)
            {
                HttpRuntime.Cache.Insert(CacheCommon.AddSysCustomKey(sysNetCacheKey, key), newValue, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(expiry.Value.Seconds));
            }
            else
            {

                HttpRuntime.Cache.Insert(key, value);
            }
            return true;
        }
        public override bool Set<T>(string key, T value,string filePath, TimeSpan? expiry = default(TimeSpan?))
        {
            var newValue = CacheCommon.ConvertJson<T>(value);
            if (expiry.HasValue)
            {
                HttpRuntime.Cache.Insert(CacheCommon.AddSysCustomKey(sysNetCacheKey, key), newValue, new CacheDependency(filePath), System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(expiry.Value.Seconds));
            }
            else
            {

                HttpRuntime.Cache.Insert(key, value, new CacheDependency(filePath));
            }
            return true;
        }
        public override bool Remove(string key)
        {
            if (KeyExists(key))
            {
                HttpRuntime.Cache.Remove(CacheCommon.AddSysCustomKey(sysNetCacheKey, key));
                return true;
            }
            else
            {
                return false;
            }
        }
        public override bool KeyExists(string key)
        {
            return HttpRuntime.Cache.Get(CacheCommon.AddSysCustomKey(sysNetCacheKey, key)) != null ? true : false;
        }
       
    }
}
