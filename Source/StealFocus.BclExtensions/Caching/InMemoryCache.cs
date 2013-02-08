// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InMemoryCache.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the InMemoryCache type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.BclExtensions.Caching
{
    using System;
    using System.Collections.Generic;

    public class InMemoryCache : ICache
    {
        private static readonly object SyncRoot = new object();

        private static readonly Dictionary<string, CacheItem> CachedItemDictionary = new Dictionary<string, CacheItem>();

        public void AddItem(string cacheKey, CacheItem cacheItem)
        {
            lock (SyncRoot)
            {
                CachedItemDictionary.Add(cacheKey, cacheItem);
            }
        }

        public bool ContainsItem(string cacheKey)
        {
            lock (SyncRoot)
            {
                bool cacheContainsItem = CachedItemDictionary.ContainsKey(cacheKey);
                if (!cacheContainsItem)
                {
                    return false;
                }

                bool cacheItemHasExpired = HasCacheItemExpired(CachedItemDictionary[cacheKey]);
                if (cacheItemHasExpired)
                {
                    this.Remove(cacheKey);
                    return false;
                }

                return CachedItemDictionary.ContainsKey(cacheKey);
            }
        }

        public object GetItem(string cacheKey)
        {
            lock (SyncRoot)
            {
                bool cacheContainsItem = CachedItemDictionary.ContainsKey(cacheKey);
                if (!cacheContainsItem)
                {
                    return null;
                }

                bool cacheItemHasExpired = HasCacheItemExpired(CachedItemDictionary[cacheKey]);
                if (cacheItemHasExpired)
                {
                    this.Remove(cacheKey);
                    return null;
                }

                return CachedItemDictionary[cacheKey].ItemValue;
            }
        }

        public void Remove(string cacheKey)
        {
            lock (SyncRoot)
            {
                CachedItemDictionary.Remove(cacheKey);
            }
        }

        public void Clear()
        {
            lock (SyncRoot)
            {
                CachedItemDictionary.Clear();
            }
        }

        private static bool HasCacheItemExpired(CacheItem cacheItem)
        {
            return cacheItem.Expiration < DateTime.Now;
        }
    }
}
