using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFire_Infrastructure.CacheHelper.RedisCacheHelper
{
    public class RedisListCache : BaseCache
    {
        private readonly ConnectionMultiplexer _conn;
        public static readonly string sysRedisKey = ConfigurationManager.AppSettings["redisKey"] ?? "";
        private IDatabase _db;


        #region 构造函数

        public RedisListCache(int dbNum = 0)
            : this(dbNum, null)
        {
        }

        public RedisListCache(int dbNum, string readWriteHosts)
        {
            _conn =
                string.IsNullOrWhiteSpace(readWriteHosts) ?
                RedisConnectionManager.Instance :
                RedisConnectionManager.GetConnectionMultiplexer(readWriteHosts);
            _db = _conn.GetDatabase(dbNum);
        }
        #endregion 构造函数
        public override bool Set<T>(string key, T value, TimeSpan? expiry = default(TimeSpan?))
        {
            var newValue = CacheCommon.ConvertJson<T>(value);
            _db.ListLeftPush(CacheCommon.AddSysCustomKey(sysRedisKey, key), newValue);
            return true;
        }
        public override T Get<T>(string key)
        {
            
            var redisValue = _db.ListRightPop(CacheCommon.AddSysCustomKey(sysRedisKey, key));
            if (string.IsNullOrWhiteSpace(redisValue)) return default(T);
            return CacheCommon.ConvertObj<T>(redisValue);
        }
        public override long GetListLength(string key)
        {

                return _db.ListLength(CacheCommon.AddSysCustomKey(sysRedisKey, key));
        }

    }
}
