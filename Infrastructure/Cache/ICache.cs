using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZR.Infrastructure.Cache
{
    public interface ICache
    {
        Task<T> GetAsync<T>(string key);

        Task<bool> SetAsync<T>(string key, T item, int? expireSeconds = null);

        Task RemoveAsync(string key);

        Task<ICacheLock> UseAsyncLock();

        ICache WithSource<T>(T source) => this;
    }

    public abstract class ICacheLock: IDisposable
    {
        public abstract void UnLock();

        public void Dispose()
        {
            UnLock();
        }
    }

    public static class ICacheExtension
    {
        public static void AddCache<Imp>(this IServiceCollection services) where Imp: class, ICache
        {
            services.AddSingleton<ICache, Imp>();
        }
    }
}
