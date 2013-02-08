// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpRuntimeCache.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the HttpRuntimeCache type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.BclExtensions.Caching
{
    using System;
    using System.Web;
    using System.Web.Caching;

    public class HttpRuntimeCache : ICache
    {
        public void AddItem(string cacheKey, CacheItem cacheItem)
        {
            if (cacheItem == null)
            {
                throw new ArgumentNullException("cacheItem");
            }

            HttpRuntime.Cache.Add(
                cacheKey, 
                cacheItem.ItemValue, 
                null, 
                cacheItem.Expiration, 
                Cache.NoSlidingExpiration, 
                CacheItemPriority.Normal, 
                null);
        }

        public bool ContainsItem(string cacheKey)
        {
            object cacheItem = HttpRuntime.Cache.Get(cacheKey);
            return cacheItem != null;
        }

        public object GetItem(string cacheKey)
        {
            return HttpRuntime.Cache.Get(cacheKey);
        }

        public void Remove(string cacheKey)
        {
            HttpRuntime.Cache.Remove(cacheKey);
        }

        public void Clear()
        {
            throw new NotSupportedException("The HTTP Runtime Cache does not support a clear operation.");
        }
    }
}
