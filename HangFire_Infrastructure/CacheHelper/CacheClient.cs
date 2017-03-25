
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFire_Infrastructure.CacheHelper
{
   public class CacheClient
    {
        private ICache _cache;
        public CacheClient(ICache cache)
        {
            this._cache = cache;
        }
        /// <summary>
        /// 数据添加到缓存(NetCache,redisStringCache,MemCache)
        /// 如果存在此key的项，会先移除再添加，如果不想执行此操作可先执行KeyExists方法（当执行memcache 保存时间最长为30天）
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool Set<T>(string key, T value, TimeSpan? expir = default(TimeSpan?))
        {
            return this._cache.Set<T>(key, value, expir);

        }
        /// <summary>
        /// 获取指定key的项(NetCache,redisStringCache,MemCache)
        /// </summary>
        /// <typeparam name="T">泛型返回值</typeparam>
        /// <param name="key">key</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            return this._cache.Get<T>(key);
        }
        /// <summary>
        /// 移除指定key的项(NetCache,redisStringCache,MemCache)
        /// </summary>
        /// <param name="key">redisKey</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            return this._cache.Remove(key);
        }
        /// <summary>
        /// 判断是否存存在此key(NetCache,redisStringCache,MemCache)
        /// </summary>
        /// <param name="key">redisKey</param>
        /// <returns></returns>
        public bool KeyExists(string key)
        {
            return this._cache.KeyExists(key);
        }
        /// <summary>
        /// Get异步方法（RedisStringCache）
        /// </summary>
        /// <typeparam name="T">返回值</typeparam>
        /// <param name="key">redisKey</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key)
        {

            return await this._cache.GetAsync<T>(key);
        }
        /// <summary>
        /// Set异步方法（RedisStringCache）
        /// </summary>
        /// <typeparam name="T">存入对象类型</typeparam>
        /// <param name="key">redisKey</param>
        /// <param name="value">存入对象</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expir = default(TimeSpan?))
        {

            return await this._cache.SetAsync<T>(key, value, expir);
        }
        /// <summary>
        /// Remove 异步方法（RedisStringCache）
        /// </summary>
        /// <param name="key">redisKey</param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(string key)
        {

            return await this._cache.RemoveAsync(key);
        }
        /// <summary>
        ///  KeyExists 异步方法（RedisStringCache）
        /// </summary>
        /// <param name="key">redisKey</param>
        /// <returns></returns>
        public async Task<bool> KeyExistsAsync(string key)
        {
            return await this._cache.KeyExistsAsync(key);
        }
        /// <summary>
        /// 数据添加到缓存（RedisHashCache）
        /// 如果存在此key的项，会先移除再添加，如果不想执行此操作可先执行KeyExists方法（当执行memcache 保存时间最长为30天）
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">redisKey</param>
        /// <param name="dataKey">redisFiledKey</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public bool Set<T>(string key,string dataKey, T value, TimeSpan? expir = default(TimeSpan?))
        {
            return this._cache.Set<T>(key,dataKey, value, expir);

        }
       /// <summary>
       /// 设置缓存(.NetCache)
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="key"></param>
       /// <param name="value"></param>
       /// <param name="filePath"></param>
       /// <param name="expir"></param>
       /// <returns></returns>
        public bool Set<T>(string key, T value, string filePath, TimeSpan? expir = default(TimeSpan?))
        {
            return this._cache.Set<T>(key, value, filePath, expir);
        }
        /// <summary>
        /// 获取指定key的（RedisHashCache）
        /// </summary>
        /// <typeparam name="T">返回值</typeparam>
        /// <param name="key">redisKey</param>
        /// <param name="dataKey">redisFiledKey</param>
        /// <returns></returns>
        public T Get<T>(string key,string dataKey)
        {
            return this._cache.Get<T>(key,dataKey);
        }
        /// <summary>
        /// 移除指定key的项（RedisHashCache）
        /// </summary>
        /// <param name="key">redisKey</param>
        /// <param name="dataKey">redisFiledKey</param>
        /// <returns></returns>
        public bool Remove(string key,string dataKey)
        {
            return this._cache.Remove(key,dataKey);
        }
        /// <summary>
        /// 判断是否存存在此key（RedisHashCache）
        /// </summary>
        /// <param name="key">redisKey</param>
        /// <param name="dataKey">redisFiledKey</param>
        /// <returns></returns>
        public bool KeyExists(string key,string dataKey)
        {
            return this._cache.KeyExists(key,dataKey);
        }
        /// <summary>
        /// Get异步方法（RedisHashCache）
        /// </summary>
        /// <typeparam name="T">返回值</typeparam>
        /// <param name="key">reidskey</param>
        /// <param name="dataKey">redisFiledKey</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key,string dataKey)
        {

            return await this._cache.GetAsync<T>(key, dataKey);
        }
        /// <summary>
        /// Set异步方法（RedisHashCache）
        /// </summary>
        /// <typeparam name="T">存入值类型</typeparam>
        /// <param name="key">redisKey</param>
        /// <param name="dataKey">redisFieldKey</param>
        /// <param name="value">存入值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public async Task<bool> SetAsync<T>(string key,string dataKey, T value, TimeSpan? expir = default(TimeSpan?))
        {

            return await this._cache.SetAsync<T>(key, dataKey, value, expir);
        }
        /// <summary>
        /// Remove 异步方法（RedisHashCache）
        /// </summary>
        /// <param name="key">redisKey</param>
        /// <param name="dataKey">redisFieldKey</param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(string key,string dataKey)
        {

            return await this._cache.RemoveAsync(key, dataKey);
        }
        /// <summary>
        ///  KeyExists 异步方法（RedisHashCache）
        /// </summary>
        /// <param name="key">redisKey</param>
        /// <param name="dataKey">redisFieldKey</param>
        /// <returns></returns>
        public async Task<bool> KeyExistsAsync(string key,string dataKey)
        {
            return await this._cache.KeyExistsAsync(key, dataKey);
        }
       /// <summary>
        /// 设置当前key的缓存时间
       /// </summary>
       /// <param name="key">redisKey</param>
       /// <param name="expir">过期时间</param>
       /// <returns></returns>
        public bool KeyExpire(string key, TimeSpan? expir = default(TimeSpan?))
        {
            return this._cache.KeyExpire(key, expir);
        }
       /// <summary>
       /// 获得redisList长度
       /// </summary>
       /// <param name="key"></param>
       /// <returns></returns>
        public long GetListLength(string key)
        {
            return this._cache.GetListLength(key);
        }

    }
}
