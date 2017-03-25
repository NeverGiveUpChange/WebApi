using Memcached.ClientLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFire_Infrastructure.CacheHelper.MemCacheHelper
{
   public class MemCache:BaseCache
    {
        private static MemcachedClient mc;
        public static readonly string sysMemCacheKey = ConfigurationManager.AppSettings["memCacheKey"] ?? "";

        public MemCache()
        {

            mc = MemcachedConnectionManager.mc;
        }
        public override T Get<T>(string key)
        {
            var value = mc.Get(CacheCommon.AddSysCustomKey(sysMemCacheKey,key));
            if (value == null) return default(T);
            if (value is string)
            {
               
                var resultValue = mc.Get(CacheCommon.AddSysCustomKey(sysMemCacheKey,key)) as string;
                return CacheCommon.ConvertObj<T>(resultValue);
            }
            else
            {
                return (T)value;
            }

        }
        public override bool Set<T>(string key, T value, TimeSpan? expiry = default(TimeSpan?))
        {
            if (KeyExists(key)) Remove(key);
            var newValue = CacheCommon.ConvertJson<T>(value);
            return expiry.HasValue ? mc.Set(CacheCommon.AddSysCustomKey(sysMemCacheKey, key), newValue, DateTime.Now.AddSeconds(expiry.Value.Seconds)) : mc.Set(CacheCommon.AddSysCustomKey(sysMemCacheKey,key), newValue);
        }

        public override bool Remove(string key)
        {
            return KeyExists(key) ? mc.Delete(CacheCommon.AddSysCustomKey(sysMemCacheKey,key)) : false;
        }
        public override bool KeyExists(string key)
        {
            return mc.KeyExists(CacheCommon.AddSysCustomKey(sysMemCacheKey,key));
        }
    }
}
