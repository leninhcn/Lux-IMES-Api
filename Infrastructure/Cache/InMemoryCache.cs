using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZR.Infrastructure.Cache
{
    public class InMemoryCache : ICache
    {
        MemoryCache Cache { get; } = new MemoryCache(new MemoryCacheOptions
        {
            //SizeLimit = 1024
        });

        public Task<T> GetAsync<T>(string key)
        {
            return Task.FromResult(Cache.Get<T>(key));
        }

        public Task RemoveAsync(string key)
        {
            Cache.Remove(key);
            return Task.CompletedTask;
        }

        public Task<bool> SetAsync<T>(string key, T item, int? expireSeconds)
        {
            if (expireSeconds == null)
                Cache.Set(key, item);
            else
                Cache.Set(key, item, DateTime.Now.AddMinutes(expireSeconds.Value));

            return Task.FromResult(true);
        }

        public Task<ICacheLock> UseAsyncLock()
        {
            return Task.FromResult<ICacheLock>(NoCacheLock.Instance);
        }
    }

    class NoCacheLock: ICacheLock
    {
        public static NoCacheLock Instance = new();
        public override void UnLock() { }
    }
}
