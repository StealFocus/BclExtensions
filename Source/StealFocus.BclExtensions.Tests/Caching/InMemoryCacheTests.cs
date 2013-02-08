// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InMemoryCacheTests.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the InMemoryCacheTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.BclExtensions.Tests.Caching
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using StealFocus.BclExtensions.Caching;

    [TestClass]
    public class InMemoryCacheTests : CacheTests<InMemoryCache>
    {
        protected override TimeSpan PauseBeforeCheckingCache
        {
            get { return new TimeSpan(0, 0, 0, 0, 0); }
        }

        [TestMethod]
        public void UnitTest_That_After_Adding_Then_Contains_Is_True_And_After_Clear_Then_Contains_Is_False()
        {
            ICache cache = new InMemoryCache();
            object cachedObject = new object();
            const string Key = "key";
            cache.AddItem(Key, new CacheItem(cachedObject, DateTime.Now.AddDays(1)));
            bool containsItem1 = cache.ContainsItem(Key);
            Assert.IsTrue(containsItem1, "Item was not indicated to be cached when it was expected to be.");
            cache.Clear();
            bool containsItem2 = cache.ContainsItem(Key);
            Assert.IsFalse(containsItem2, "Item was indicated to be cached when it was not expected to be.");
        }
    }
}
