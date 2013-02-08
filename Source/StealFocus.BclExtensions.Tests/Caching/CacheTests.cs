// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheTests.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the CacheTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.BclExtensions.Tests.Caching
{
    using System;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using StealFocus.BclExtensions.Caching;

    [TestClass]
    public abstract class CacheTests<T> where T : ICache, new()
    {
        private const string CacheKeyOfItemThatExists = "CacheKeyOfItemThatExists";

        private const string CacheKeyOfItemThatDoesNotExist = "CacheKeyOfItemThatDoesNotExist";

        private const string CacheKeyOfItemThatHasExpired = "CacheKeyOfItemThatHasExpired";

        private readonly TimeSpan oneDay = new TimeSpan(1, 0, 0);

        /// <remarks>
        /// Some of the cache providers (like HTTP Runtime) require 
        /// a moment to remove an expired item from the cache. For 
        /// those providers an appropriate (brief) value should be
        /// set. For other providers this can be set to zero.
        /// </remarks>
        protected abstract TimeSpan PauseBeforeCheckingCache { get; }

        [TestMethod]
        public void UnitTest_That_After_Adding_Then_Contains_Is_True_And_Get_Returns_The_Same_Item()
        {
            T cache = new T();
            object cachedObject = new object();
            cache.AddItem(CacheKeyOfItemThatExists, new CacheItem(cachedObject, DateTime.Now.AddDays(1)));
            bool containsItem = cache.ContainsItem(CacheKeyOfItemThatExists);
            Assert.IsTrue(containsItem, "Item was not indicated to be cached when it was expected to be.");
            object itemFromCache = cache.GetItem(CacheKeyOfItemThatExists);
            Assert.AreEqual(cachedObject, itemFromCache, "The item retrieved from the cache did not match the one put in.");
        }

        [TestMethod]
        public void UnitTest_That_Without_Adding_Then_Contains_Is_False_And_Get_Returns_Null()
        {
            T cache = new T();
            bool containsItem = cache.ContainsItem(CacheKeyOfItemThatDoesNotExist);
            Assert.IsFalse(containsItem, "Item was indicated to be cached when it was not expected to be.");
            object itemFromCache = cache.GetItem(CacheKeyOfItemThatDoesNotExist);
            Assert.IsNull(itemFromCache, "An item was retrieved from the cache when it was not expected to be.");
        }

        [TestMethod]
        public void UnitTest_That_After_Adding_Then_Expiring_Contains_Is_False()
        {
            T cache = new T();
            object cachedObject = new object();
            cache.AddItem(CacheKeyOfItemThatHasExpired, new CacheItem(cachedObject, DateTime.Now.Subtract(this.oneDay)));
            Thread.Sleep(this.PauseBeforeCheckingCache);
            bool containsItem = cache.ContainsItem(CacheKeyOfItemThatHasExpired);
            Assert.IsFalse(containsItem, "Item was indicated to be cached when it was not expected to be (it should have expired).");
        }

        [TestMethod]
        public void UnitTest_That_After_Adding_Then_Expiring_Get_Is_Null()
        {
            T cache = new T();
            object cachedObject = new object();
            cache.AddItem(CacheKeyOfItemThatHasExpired, new CacheItem(cachedObject, DateTime.Now.Subtract(this.oneDay)));
            Thread.Sleep(this.PauseBeforeCheckingCache);
            object itemFromCache = cache.GetItem(CacheKeyOfItemThatHasExpired);
            Assert.IsNull(itemFromCache, "An item was retrieved from the cache when it was not expected to be (it should have expired).");
        }
    }
}
