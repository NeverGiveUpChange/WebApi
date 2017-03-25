using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFire_Infrastructure.CacheHelper
{
  public abstract class BaseCache:ICache
    {
        public virtual T Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public virtual bool Set<T>(string key, T value, TimeSpan? expiry = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public virtual bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public virtual bool KeyExists(string key)
        {
            throw new NotImplementedException();
        }

        public virtual  Task<T> GetAsync<T>(string key)
        {
            throw   new NotImplementedException();
        }

        public virtual Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> RemoveAsync(string key)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> KeyExistsAsync(string key)
        {
            throw new NotImplementedException();
        }

        public virtual T Get<T>(string key, string dataKey)
        {
            throw new NotImplementedException();
        }

        public virtual bool Set<T>(string key, string dataKey, T value, TimeSpan? expiry = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public virtual bool Remove(string key, string dataKey)
        {
            throw new NotImplementedException();
        }

        public virtual bool KeyExists(string key, string dataKey)
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> GetAsync<T>(string key, string dataKey)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> SetAsync<T>(string key, string dataKey, T value, TimeSpan? expiry = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> RemoveAsync(string key, string dataKey)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> KeyExistsAsync(string key, string dataKey)
        {
            throw new NotImplementedException();
        }


        public virtual bool Set<T>(string key, T value,string filePath, TimeSpan? expiry = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }


        public virtual  bool KeyExpire(string key, TimeSpan? expiry = default(TimeSpan?))
        {
            throw new NotImplementedException();
        }


        public virtual long GetListLength(string key)
        {
            throw new NotImplementedException();
        }
    }
}
