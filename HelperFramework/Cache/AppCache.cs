using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace HelperFramework.Cache
{
    public static class AppCache 
    {
        private static readonly ObjectCache Cache = MemoryCache.Default;
        
        public static T Get<T>(string cacheEntryName) where T: class
        {
            try
            {
                object cacheObj = Cache[cacheEntryName];

                if (cacheObj == null)
                {
                    return null;
                }

                return (T)cacheObj;
            }
            catch
            {
                return null;
            }
        }

        public static bool Contains(string cacheEntryName)
        {
            return Cache.Contains(cacheEntryName) && Cache.Get(cacheEntryName) != null;
        }

        public static void Insert(string cacheEntryName, Object cacheEntry)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30);
            Insert(cacheEntryName, cacheEntry, policy);
        }

        public static Object Insert(string cacheEntryName, Object cacheEntry, CacheItemPolicy policy)
        {
            Cache.Add(cacheEntryName, cacheEntry, policy);
            return cacheEntry;
        }

    }
}
