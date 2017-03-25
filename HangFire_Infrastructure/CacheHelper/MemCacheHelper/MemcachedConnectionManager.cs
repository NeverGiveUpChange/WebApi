using Memcached.ClientLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFire_Infrastructure.CacheHelper.MemCacheHelper
{
   internal class MemcachedConnectionManager
    {
       /// <summary>
       /// 定义一个静态MemcachedClient客户端,它随类一起加载，所有对象共用
       /// </summary>
       public static MemcachedClient mc;
       /// <summary>
       /// 静态构造函数，初始化Memcached客户端
       /// </summary>
       static MemcachedConnectionManager()
       {
           string[] serverList = { "127.0.0.1:11211" };
           SockIOPool pool = SockIOPool.GetInstance();
           pool.SetServers(serverList);
           pool.Initialize();
           //TODO:部分设置项不走默认须自己配置
           mc = new MemcachedClient();
           mc.EnableCompression = false;
       }
    }
}
