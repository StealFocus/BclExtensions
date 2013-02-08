// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheItem.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the CacheItem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.BclExtensions.Caching
{
    using System;

    public class CacheItem
    {
        public CacheItem()
        {
        }

        public CacheItem(object itemValue, DateTime expiration)
        {
            this.ItemValue = itemValue;
            this.Expiration = expiration;
        }

        public object ItemValue { get; set; }

        public DateTime Expiration { get; set; }
    }
}
