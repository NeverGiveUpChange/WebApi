using CacheManager.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFire_CacheManager
{
    class Class1
    {

        public Class1()
        {
            var cache = CacheFactory.Build<int>("myCache", settings =>
 {
     settings
.WithSystemRuntimeCacheHandle("inProcessCache")//内存缓存Handle
.And
.WithRedisConfiguration("redis", config =>//Redis缓存配置
{
config.WithAllowAdmin()
.WithDatabase(0)
.WithEndpoint("localhost", 6379);
})
.WithMaxRetries(1000)//尝试次数
.WithRetryTimeout(100)//尝试超时时间
.WithRedisBackplane("redis")//redis使用Back Plate
.WithRedisCacheHandle("redis", true);//redis缓存handle
 });
           
        }

    }
}
