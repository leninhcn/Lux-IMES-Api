using CSRedis;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZR.Common.Cache;

namespace ZR.Infrastructure.Cache
{
    public class RedisCache : ICache
    {
        RedisSourceCache DefaultSource => Cache;
        RedisSourceCache Cache = new(RedisServer.Cache, "Cache-Global");
        RedisSourceCache Session = new(RedisServer.Session, "Session-Global");

        public async Task<T> GetAsync<T>(string key) => await DefaultSource.GetAsync<T>(key);

        public async Task RemoveAsync(string key) => await DefaultSource.RemoveAsync(key);

        public async Task<bool> SetAsync<T>(string key, T item, int? expireSeconds)
            => await DefaultSource.SetAsync(key, item, expireSeconds);

        public ICache WithSource<T>(T obj) {
            if (obj is not CSRedisClient source) return this;

            if (source == RedisServer.Cache)
                return Cache;
            else if (source == RedisServer.Session)
                return Session;
            else return this;
        }

        public Task<ICacheLock> UseAsyncLock() => DefaultSource.UseAsyncLock();
    }

    class RedisCacheLock : ICacheLock
    {
        CSRedisClient client;
        CSRedisClientLock lockObj;
        public RedisCacheLock(CSRedisClient client)
        {
            this.client = client;
        }

        internal async Task Lock(string name)
        {
            var lockObj = this.lockObj;
            while (lockObj is null ) {
                lockObj = await client.TryLockAsync(name, 1);
            }
            this.lockObj = lockObj;
        }

        public override void UnLock()
        {
            lockObj?.Unlock();
        }
    }

    class RedisSourceCache : ICache
    {
        public RedisSourceCache(CSRedisClient source, string name)
        {
            this.source = source;
            this.name = name;
        }

        readonly CSRedisClient source;
        readonly string name;
        public async Task<T> GetAsync<T>(string key)
        {
            return await source.GetAsync<T>(key);
        }

        public async Task RemoveAsync(string key)
        {
            await source.DelAsync(key);
        }

        public async Task<bool> SetAsync<T>(string key, T item, int? expireSeconds)
        {
            return await source.SetAsync(key, item, expireSeconds ?? -1);
        }

        public async Task<ICacheLock> UseAsyncLock()
        {
            var cacheLock = new RedisCacheLock(source);
            await cacheLock.Lock(name);
            return cacheLock;
        }
    }

    class EmptyDispose : IDisposable
    {
        public static EmptyDispose Instance = new();
        public void Dispose() { }
    }

    static class CSRedisClientExtension {
        public static async Task<CSRedisClientLock> TryLockAsync(this CSRedisClient client, 
            string name, int timeoutSeconds, bool autoDelay = true)
        {
            name = "CSRedisClientLock:" + name;
            string value = Guid.NewGuid().ToString();
            if (await client.SetAsync(name, value, timeoutSeconds, RedisExistence.Nx))
            {
                double refreshSeconds = (double)timeoutSeconds / 2.0;
                return new CSRedisClientLock(client, name, value, timeoutSeconds, refreshSeconds, autoDelay);
            }

            return null;
        }
    }
}
