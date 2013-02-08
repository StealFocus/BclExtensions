// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpRuntimeCacheTests.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the HttpRuntimeCacheTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.BclExtensions.Tests.Caching
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using StealFocus.BclExtensions.Caching;

    [TestClass]
    public class HttpRuntimeCacheTests : CacheTests<HttpRuntimeCache>
    {
        protected override TimeSpan PauseBeforeCheckingCache
        {
            get { return new TimeSpan(0, 0, 0, 0, 500); }
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void UnitTest_That_After_Adding_Then_Contains_Is_True_And_Clear_Throws_NotSupportedException()
        {
            ICache cache = new HttpRuntimeCache();
            object cachedObject = new object();
            const string Key = "key";
            cache.AddItem(Key, new CacheItem(cachedObject, DateTime.Now.AddDays(1)));
            bool containsItem1 = cache.ContainsItem(Key);
            Assert.IsTrue(containsItem1, "Item was not indicated to be cached when it was expected to be.");
            cache.Clear();
        }
    }
}
