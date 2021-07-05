using System;
using System.Runtime.Caching;

namespace NEWS_MVC.Helpers
{
    public static class MemoryCacheHelper
    {
        /// <summary>
        /// Get cache value by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetValue<T>(string key)
        {
            try
            {
                return (T)MemoryCache.Default.Get(key);
            }
            catch
            {
                return default(T);
            }
        }
        /// <summary>
        /// Get cache value by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetValue(string key)
        {
            try
            {
                return MemoryCache.Default.Get(key);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Add a cache object with date expiration
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absExpiration"></param>
        /// <returns></returns>
        public static bool Add(string key, object value, DateTimeOffset? absExpiration = null)
        {
            absExpiration ??= DateTime.Now.AddHours(1);
            MemoryCache.Default.Remove(key);
            return MemoryCache.Default.Add(key, value, absExpiration.Value);
        }

        /// <summary>
        /// Delete cache value from key
        /// </summary>
        /// <param name="key"></param>
        public static void Delete(string key)
        {
            MemoryCache memoryCache = MemoryCache.Default;
            if (memoryCache.Contains(key))
            {
                memoryCache.Remove(key);
            }
        }
        /// <summary>
        /// Delete all
        /// </summary>
        public static void Clear()
        {
            foreach (var element in MemoryCache.Default)
            {
                MemoryCache.Default.Remove(element.Key);
            }
        }  
    }
}
