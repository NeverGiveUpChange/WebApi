using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFire_CacheManager
{
   public class CacheManagerInitParameter
    {
        private static string _cacheName= "redis";
        private static string _instanceName = "myCache";

        private static string _systemCacheHandleName = "inProcessCache";
        private static string _redisConnectionString = "127.0.0.1";
        private static int _redisEndPoint = 6379;
        private static int _dbNum = 0;
        /// <summary>
        /// 缓存管理实例名称
        /// </summary>
        public static string InstanceName { get => _instanceName; set => _instanceName = value; }
        /// <summary>
        /// 系统内存缓存句柄名称
        /// </summary>
        public static string SystemCacheHandleName { get => _systemCacheHandleName; set => _systemCacheHandleName = value; }
        /// <summary>
        /// redis连接字符串
        /// </summary>
        public static string RedisConnectionString { get => _redisConnectionString; set => _redisConnectionString = value; }
        /// <summary>
        /// redis端口
        /// </summary>
        public static int RedisEndPoint { get => _redisEndPoint; set => _redisEndPoint = value; }
        /// <summary>
        /// redis存储Db号
        /// </summary>
        public static int DbNum { get => _dbNum; set => _dbNum = value; }
        public static string CacheName { get => _cacheName; private set => _cacheName = value; }
    }
}
