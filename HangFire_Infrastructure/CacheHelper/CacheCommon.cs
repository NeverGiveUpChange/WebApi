using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFire_Infrastructure.CacheHelper
{
   internal class CacheCommon
    {

        public static string AddSysCustomKey(string cacheTypeKey, string oldKey)
        {
            return cacheTypeKey + oldKey;
        }

        public static string ConvertJson<T>(T value)
        {
            string result = value is string ? value.ToString() : JsonConvert.SerializeObject(value);
            return result;
        }

        public static T ConvertObj<T>(RedisValue value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
        public static T ConvertObj<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        private RedisKey[] ConvertRedisKeys(List<string> redisKeys)
        {
            return redisKeys.Select(redisKey => (RedisKey)redisKey).ToArray();
        }
    }
}
