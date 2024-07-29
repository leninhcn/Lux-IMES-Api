using Infrastructure;
using Infrastructure.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ZR.Common;
using ZR.Common.Cache;
using ZR.Infrastructure.Cache;

namespace ZR.Service.System
{
    public class CacheService
    {
        static readonly Lazy<ICache> LazyCache = new(() => InternalApp.ServiceProvider.GetService<ICache>(), true);
        static ICache Cache => LazyCache.Value;

        public static Task Cach { get; private set; }

        private readonly static string CK_verifyScan = "verifyScan_";
        #region 用户权限 缓存
        public static async Task<List<string>> GetUserPerms(string key)
        {
            return await Cache.GetAsync<List<string>>(key);
            //return RedisServer.Cache.Get<List<string>>(key).ToList();
        }

        public static async Task SetUserPerms(string key, object data)
        {
            await Cache.SetAsync(key, data);
            //RedisServer.Cache.Set(key, data);
        }
        public static async Task RemoveUserPerms(string key)
        {
            await Cache.RemoveAsync(key);
            //RedisServer.Cache.Del(key);
        }
        #endregion

        #region 用户 Session 缓存

        static readonly int SessionExpire;

        static CacheService()
        {
            var sessionExpire = AppSettings.App("UserSession", "Expire");
            if (!int.TryParse(sessionExpire, out SessionExpire))
                SessionExpire = 30;
        }

        public static async Task<ICacheLock> UseSessionCacheLock()
        {
            return await Cache.WithSource(RedisServer.Session).UseAsyncLock();
        }

        public static async Task<bool> SetUserToken(string uid, TokenModel tokenModel)
        {
            var key = $"SessionUid.{uid}";
            tokenModel.ExpireTime = DateTime.UtcNow.AddMinutes(SessionExpire);
            return await Cache.WithSource(RedisServer.Session).SetAsync(key, tokenModel, SessionExpire * 60);
        }

        public static async Task<bool> CheckUserToken(string uid)
        {
            var tokenModel = await GetUserToken(uid);
            return tokenModel != null;
        }

        public static async Task<TokenModel> GetUserToken(string uid)
        {
            var key = $"SessionUid.{uid}";
            return await Cache.WithSource(RedisServer.Session).GetAsync<TokenModel>(key);
        }

        public static async Task RemoveUserToken(string uid)
        {
            var key = $"SessionUid.{uid}";
            await Cache.WithSource(RedisServer.Session).RemoveAsync(key);
        }

        #endregion

        public static object SetScanLogin(string key, Dictionary<string, object> val)
        {
            var ck = CK_verifyScan + key;
            
            return CacheHelper.SetCache(ck,val , 1);
        }
        public static object GetScanLogin(string key)
        {
            var ck = CK_verifyScan + key;
            return CacheHelper.Get(ck);
        }
        public static void RemoveScanLogin(string key)
        {
            var ck = CK_verifyScan + key;
            CacheHelper.Remove(ck);
        }

        public static void SetLockUser(string key, long val, int time)
        {
            var CK = "lock_user_" + key;

            CacheHelper.SetCache(CK, val, time);
        }

        public static long GetLockUser(string key)
        {
            var CK = "lock_user_" + key;

            if (CacheHelper.Get(CK) is long t)
            {
                return t;
            }
            return 0;
        }
    }
}
