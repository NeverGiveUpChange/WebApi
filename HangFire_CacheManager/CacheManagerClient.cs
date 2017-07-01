using CacheManager.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFire_CacheManager
{
    public class CacheManagerClient
    {
        private static readonly ConcurrentDictionary<string, ICacheManager<object>> _connectionCache = new ConcurrentDictionary<string, ICacheManager<object>>();
        private static readonly object Locker = new object();
        private static string connectionCacheKey = string.Empty;
        private CacheManagerClient()
        { }
        public static ICacheManager<object> Instance
        {
            get
            {
                connectionCacheKey = GetConnectionCacheKey();
                if (_connectionCache[connectionCacheKey] == null)
                {
                    lock (Locker)
                    {
                        if (_connectionCache[connectionCacheKey] == null)
                        {
                            _connectionCache[connectionCacheKey] =
                            GetCacheManagerClient();
                        }
                    }
                }
                return _connectionCache[connectionCacheKey];

            }
        }
        private static ICacheManager<object> GetCacheManagerClient()
        {
                _connectionCache[connectionCacheKey] = CacheFactory.Build(CacheManagerInitParameter.InstanceName, settings =>
                {
                    settings.WithUpdateMode(CacheUpdateMode.Up)
               .WithSystemRuntimeCacheHandle(CacheManagerInitParameter.SystemCacheHandleName)//内存缓存Handle
               .WithExpiration(ExpirationMode.Sliding, TimeSpan.FromSeconds(60))//默认内存缓存时间
               .And
               .WithRedisConfiguration(CacheManagerInitParameter.CacheName, config =>//Redis缓存配置
           {
               config.WithAllowAdmin()
                          .WithDatabase(CacheManagerInitParameter.DbNum)
                          .WithEndpoint(CacheManagerInitParameter.RedisConnectionString, CacheManagerInitParameter.RedisEndPoint);
           })
               .WithMaxRetries(1000)//尝试次数
               .WithRetryTimeout(100)//尝试超时时间
               .WithRedisBackplane(CacheManagerInitParameter.CacheName)//redis使用Back Plate
               .WithRedisCacheHandle(CacheManagerInitParameter.CacheName, true)    //redis缓存handle
               .WithExpiration(ExpirationMode.Sliding, TimeSpan.FromHours(24));//默认redis缓存时间
                });
            return _connectionCache[connectionCacheKey];
        }
        private static string GetConnectionCacheKey()
        {
            return CacheManagerInitParameter.RedisConnectionString + "_" + CacheManagerInitParameter.RedisEndPoint;
        }

    }
}
