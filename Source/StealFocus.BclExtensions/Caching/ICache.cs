// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICache.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the ICache type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.BclExtensions.Caching
{
    public interface ICache
    {
        void AddItem(string cacheKey, CacheItem cacheItem);

        bool ContainsItem(string cacheKey);

        object GetItem(string cacheKey);

        void Remove(string cacheKey);

        void Clear();
    }
}
