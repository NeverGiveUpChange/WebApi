using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFire_Infrastructure.CacheHelper.RedisCacheHelper
{
    public class RedisZSetCache : BaseCache
    {
        private readonly ConnectionMultiplexer _conn;
        public static readonly string sysRedisKey = ConfigurationManager.AppSettings["redisKey"] ?? "";
        private IDatabase _db;


        #region 构造函数

        public RedisZSetCache(int dbNum = 0)
            : this(dbNum, null)
        {
        }
        public RedisZSetCache(int dbNum, string readWriteHosts)
        {
            _conn =
                string.IsNullOrWhiteSpace(readWriteHosts) ?
                RedisConnectionManager.Instance :
                RedisConnectionManager.GetConnectionMultiplexer(readWriteHosts);
            _db = _conn.GetDatabase(dbNum);
            
        }
        #endregion 构造函数
        public override bool Set<T>(string key, T value, double score, TimeSpan? expiry = default(TimeSpan?))
        {

            var newValue = CacheCommon.ConvertJson<T>(value);
            return _db.SortedSetAdd(CacheCommon.AddSysCustomKey(sysRedisKey, key), newValue, score);

        }
        public override List<T> Get<T>(string key, double startScore, double endScore)
        {
            var redisValues = _db.SortedSetRangeByScore(CacheCommon.AddSysCustomKey(sysRedisKey, key), startScore, endScore); //zrangebyscore
            if (redisValues.Length <= 0) return default(List<T>);
            return CacheCommon.ConvertObj<List<T>>(CacheCommon.ConvertJson(redisValues));
        }
        public override long Remove(string key, double startScore, double endScore)
        {
            return _db.SortedSetRemoveRangeByScore(key, startScore, endScore);
        }
    }
}
