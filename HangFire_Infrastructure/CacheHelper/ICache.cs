using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFire_Infrastructure.CacheHelper
{
   public interface ICache
   {   
       /// <summary>
       /// 获取指定key的项
       /// </summary>
       /// <typeparam name="T">泛型返回值</typeparam>
       /// <param name="key">key</param>
       /// <returns></returns>
       T Get<T>(string key);
       /// <summary>
       /// 数据添加到缓存
       /// 如果存在此key的项，会先移除再添加，如果不想执行此操作可先执行KeyExists方法（当执行memcache 保存时间最长为30天）
       /// </summary>
       /// <typeparam name="T">值类型</typeparam>
       /// <param name="key">key</param>
       /// <param name="value">值</param>
       /// <param name="expiry">过期时间</param>
       /// <returns></returns>
       bool Set<T>(string key, T value, TimeSpan? expiry = default(TimeSpan?));
       /// <summary>
       /// 数据添加到缓存（.NetCache）
       /// </summary>
       /// <typeparam name="T">值类型</typeparam>
       /// <param name="key">key</param>
       /// <param name="value">值</param>
       /// <param name="expiry">过期时间</param>
       /// <param name="filePath">文件路径</param>
       /// <returns></returns>
       bool Set<T>(string key, T value,string filePath, TimeSpan? expiry = default(TimeSpan?));
       /// <summary>
       /// 移除指定key的项
       /// </summary>
       /// <param name="key">redisKey</param>
       /// <returns></returns>
       bool Remove(string key);
       /// <summary>
       /// 判断是否存存在此key
       /// </summary>
       /// <param name="key">redisKey</param>
       /// <returns></returns>
       bool KeyExists(string key);
       /// <summary>
       /// Get异步方法（redis string）
       /// </summary>
       /// <typeparam name="T">返回值</typeparam>
       /// <param name="key">redisKey</param>
       /// <returns></returns>
       Task<T> GetAsync<T>(string key);
       /// <summary>
       /// Set异步方法（redis string）
       /// </summary>
       /// <typeparam name="T">存入对象类型</typeparam>
       /// <param name="key">redisKey</param>
       /// <param name="value">存入对象</param>
       /// <param name="expiry">过期时间</param>
       /// <returns></returns>
       Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = default(TimeSpan?));
       /// <summary>
       /// Remove 异步方法（redis string）
       /// </summary>
       /// <param name="key">redisKey</param>
       /// <returns></returns>
       Task<bool> RemoveAsync(string key);
       /// <summary>
       ///  KeyExists 异步方法（redis  string）
       /// </summary>
       /// <param name="key">redisKey</param>
       /// <returns></returns>
       Task<bool> KeyExistsAsync(string key);



       /// <summary>
       /// 获取指定key的(redis hash)
       /// </summary>
       /// <typeparam name="T">返回值</typeparam>
       /// <param name="key">redisKey</param>
       /// <param name="dataKey">redisFiledKey</param>
       /// <returns></returns>
       T Get<T>(string key,string dataKey);
       /// <summary>
       /// 数据添加到缓存(redis hash)
       /// 如果存在此key的项，会先移除再添加，如果不想执行此操作可先执行KeyExists方法（当执行memcache 保存时间最长为30天）
       /// </summary>
       /// <typeparam name="T">值类型</typeparam>
       /// <param name="key">redisKey</param>
       /// <param name="dataKey">redisFiledKey</param>
       /// <param name="value">值</param>
       /// <param name="expiry">过期时间</param>
       /// <returns></returns>
       bool Set<T>(string key,string dataKey, T value, TimeSpan? expiry = default(TimeSpan?));
       /// <summary>
       /// 移除指定key的项(redis hash)
       /// </summary>
       /// <param name="key">redisKey</param>
       /// <param name="dataKey">redisFiledKey</param>
       /// <returns></returns>
       bool Remove(string key,string dataKey);
       /// <summary>
       /// 判断是否存存在此key(redis hash)
       /// </summary>
       /// <param name="key">redisKey</param>
       /// <param name="dataKey">redisFiledKey</param>
       /// <returns></returns>
       bool KeyExists(string key,string dataKey);
       /// <summary>
       /// 根据键设置缓存时间
       /// </summary>
       /// <param name="key">redisKey</param>
       /// <param name="expiry">过期时间</param>
       /// <returns></returns>
       bool KeyExpire(string key, TimeSpan? expiry = default(TimeSpan?));
       /// <summary>
       /// Get异步方法（redis hash）
       /// </summary>
       /// <typeparam name="T">返回值</typeparam>
       /// <param name="key">reidskey</param>
       /// <param name="dataKey">redisFiledKey</param>
       /// <returns></returns>
       Task<T> GetAsync<T>(string key,string dataKey);
       /// <summary>
       /// Set异步方法（redis hash）
       /// </summary>
       /// <typeparam name="T">存入值类型</typeparam>
       /// <param name="key">redisKey</param>
       /// <param name="dataKey">redisFieldKey</param>
       /// <param name="value">存入值</param>
       /// <param name="expiry">过期时间</param>
       /// <returns></returns>
       Task<bool> SetAsync<T>(string key,string dataKey, T value, TimeSpan? expiry = default(TimeSpan?));
       /// <summary>
       /// Remove 异步方法（redis hash）
       /// </summary>
       /// <param name="key">redisKey</param>
       /// <param name="dataKey">redisFieldKey</param>
       /// <returns></returns>
       Task<bool> RemoveAsync(string key,string dataKey);
       /// <summary>
       ///  KeyExists 异步方法（redis hash）
       /// </summary>
       /// <param name="key">redisKey</param>
       /// <param name="dataKey">redisFieldKey</param>
       /// <returns></returns>
       Task<bool> KeyExistsAsync(string key,string dataKey);
       /// <summary>
       /// 获得list的长度
       /// </summary>
       /// <param name="key"></param>
       /// <returns></returns>
       long GetListLength(string key);
        /// <summary>
        /// 添加sortedset
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">redisKey</param>
        /// <param name="value">值</param>
        /// <param name="score">排序分</param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        bool Set<T>(string key, T value, double score, TimeSpan? expiry = default(TimeSpan?));
        /// <summary>
        /// 获取sortedset根据开始分，和结束分
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">redisKey</param>
        /// <param name="startScore">开始分</param>
        /// <param name="endScore">结束分</param>
        /// <returns></returns>

        List<T> Get<T>(string key, double startScore, double endScore);
        /// <summary>
        /// 移除被消费的项
        /// </summary>
        /// <param name="key">redisKey</param>
        /// <param name="startScore">开始分</param>
        /// <param name="endScore">结束分</param>
        /// <returns></returns>

        long Remove(string key, double startScore, double endScore);
      
    }
}
