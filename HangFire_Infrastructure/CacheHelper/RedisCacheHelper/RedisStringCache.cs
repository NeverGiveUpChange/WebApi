using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFire_Infrastructure.CacheHelper.RedisCacheHelper
{
   public class RedisStringCache:BaseCache
    {  
    
        private readonly ConnectionMultiplexer _conn;
        public static readonly string sysRedisKey = ConfigurationManager.AppSettings["redisKey"] ?? "";
        private IDatabase _db;

        #region 构造函数

        public RedisStringCache(int dbNum = 0)
            : this(dbNum, null)
        {
        }

        public RedisStringCache(int dbNum, string readWriteHosts)
        {
         
            _conn =
                string.IsNullOrWhiteSpace(readWriteHosts) ?
                RedisConnectionManager.Instance :
                RedisConnectionManager.GetConnectionMultiplexer(readWriteHosts);
            _db = _conn.GetDatabase(dbNum);
        }
        #endregion 构造函数
     
        public override T Get<T>(string key)
        {
     
            var redisValue = _db.StringGet(CacheCommon.AddSysCustomKey(sysRedisKey, key));

            if (string.IsNullOrWhiteSpace(redisValue)) return default(T);
            return CacheCommon.ConvertObj<T>(redisValue);
        }

        public override bool Set<T>(string key, T value, TimeSpan? expiry = default(TimeSpan?))
        {
            if (KeyExists(key)) Remove(key);
            var newValue = CacheCommon.ConvertJson<T>(value);
    
            return _db.StringSet(CacheCommon.AddSysCustomKey(sysRedisKey, key), newValue, expiry);
        }
        public override bool Remove(string key)
        {
          
            return KeyExists(key) ? _db.KeyDelete(CacheCommon.AddSysCustomKey(sysRedisKey, key)) : false;

        }
        public override bool KeyExists(string key)
        {
           
            return _db.KeyExists(CacheCommon.AddSysCustomKey(sysRedisKey, key));
        }


        public override async Task<T> GetAsync<T>(string key)
        {
           
            var redisValue = await _db.StringGetAsync(CacheCommon.AddSysCustomKey(sysRedisKey, key));

            if (string.IsNullOrWhiteSpace(redisValue)) return default(T);
            return CacheCommon.ConvertObj<T>(redisValue);
        }

        public override async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = default(TimeSpan?))
        {
            if (KeyExists(key)) Remove(key);
            var newValue = CacheCommon.ConvertJson<T>(value);
         
            return await _db.StringSetAsync(CacheCommon.AddSysCustomKey(sysRedisKey, key), newValue, expiry);
        }

        public override async Task<bool> RemoveAsync(string key)
        {
          
            return KeyExists(key) ? await _db.KeyDeleteAsync(CacheCommon.AddSysCustomKey(sysRedisKey, key)) : false;
        }

        public override async Task<bool> KeyExistsAsync(string key)
        {
          
            return  await _db.KeyExistsAsync(CacheCommon.AddSysCustomKey(sysRedisKey, key));
        }
        public override bool KeyExpire(string key, TimeSpan? expiry = default(TimeSpan?))
        {
            return _db.KeyExpire(CacheCommon.AddSysCustomKey(sysRedisKey, key), expiry);
        }
    }
}
