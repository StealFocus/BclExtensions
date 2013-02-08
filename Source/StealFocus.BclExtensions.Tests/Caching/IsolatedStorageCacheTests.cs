// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IsolatedStorageCacheTests.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the IsolatedStorageCacheTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.BclExtensions.Tests.Caching
{
    using System;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Xml;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using StealFocus.BclExtensions.Caching;

    [TestClass]
    public class IsolatedStorageCacheTests : CacheTests<IsolatedStorageCache>
    {
        protected override TimeSpan PauseBeforeCheckingCache
        {
            get { return new TimeSpan(0, 0, 0, 0, 0); }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            IsolatedStorageCache.ClearAll();
        }

        [TestMethod]
        public void UnitTest_That_After_Adding_Then_Contains_Is_True_And_After_Clear_Then_Contains_Is_False()
        {
            ICache cache = new IsolatedStorageCache();
            object cachedObject = new object();
            const string Key = "key";
            cache.AddItem(Key, new CacheItem(cachedObject, DateTime.Now.AddDays(1)));
            bool containsItem1 = cache.ContainsItem(Key);
            Assert.IsTrue(containsItem1, "Item was not indicated to be cached when it was expected to be.");
            cache.Clear();
            bool containsItem2 = cache.ContainsItem(Key);
            Assert.IsFalse(containsItem2, "Item was indicated to be cached when it was not expected to be.");
        }

        [TestMethod]
        public void UnitTest_Add_To_Cache_And_Retrieve_From_Isolated_Storage_With_A_Different_Cache_Instance()
        {
            const string MyTestFileName = "MyTest1";
            const string MyCacheKey = "MyKey";
            const int PropertyOneValue = 123;
            const string PropertyTwoValue = "ABC";
            CacheItem cacheItem = new CacheItem(new ComplexTypeForAddingToCache { PropertyOne = PropertyOneValue, PropertyTwo = PropertyTwoValue }, DateTime.Now.AddDays(1));
            ICache cache1 = new IsolatedStorageCache(MyTestFileName);
            cache1.AddItem(MyCacheKey, cacheItem);
            ICache cache2 = new IsolatedStorageCache(MyTestFileName);
            bool containsItem = cache2.ContainsItem(MyCacheKey);
            Assert.IsTrue(containsItem, "The cache was not indicated to contain the item when it was expected to be.");
            ComplexTypeForAddingToCache complexTypeForAddingToCache = (ComplexTypeForAddingToCache)cache2.GetItem(MyCacheKey);
            Assert.AreEqual(PropertyOneValue, complexTypeForAddingToCache.PropertyOne, "PropertyOne was not as expected after getting from the cache.");
            Assert.AreEqual(PropertyTwoValue, complexTypeForAddingToCache.PropertyTwo, "PropertyTwo was not as expected after getting from the cache.");
        }

        [TestMethod]
        public void UnitTest_Add_Lots_Of_Items_To_Cache_And_Retrieve_From_Isolated_Storage_With_A_Different_Cache_Instance()
        {
            const string MyTestFileName = "MyTest2";
            const string MyCacheKey1 = "MyKey1";
            const string MyCacheKey2 = "MyKey2";
            const string MyCacheKey3 = "MyKey3";
            const string MyCacheKey4 = "MyKey4";
            
            // Add items to cache
            ICache cache1 = new IsolatedStorageCache(MyTestFileName);
            CacheItem cacheItem1 = new CacheItem(new ComplexTypeForAddingToCache { PropertyOne = 1, PropertyTwo = "a" }, DateTime.Now.AddDays(1));
            cache1.AddItem(MyCacheKey1, cacheItem1);
            CacheItem cacheItem2 = new CacheItem(DateTime.Now, DateTime.Now.AddDays(1));
            cache1.AddItem(MyCacheKey2, cacheItem2);
            CacheItem cacheItem3 = new CacheItem(1, DateTime.Now.AddDays(1));
            cache1.AddItem(MyCacheKey3, cacheItem3);
            CacheItem cacheItem4 = new CacheItem("abc", DateTime.Now.AddDays(1));
            cache1.AddItem(MyCacheKey4, cacheItem4);
            
            // Get items from cache
            ICache cache2 = new IsolatedStorageCache(MyTestFileName);
            bool containsItem1 = cache2.ContainsItem(MyCacheKey1);            
            Assert.IsTrue(containsItem1, "The cache was not indicated to contain item 1 when it was expected to be.");
            bool containsItem2 = cache2.ContainsItem(MyCacheKey2);
            Assert.IsTrue(containsItem2, "The cache was not indicated to contain item 2 when it was expected to be.");
            bool containsItem3 = cache2.ContainsItem(MyCacheKey3);
            Assert.IsTrue(containsItem3, "The cache was not indicated to contain item 3 when it was expected to be.");
            bool containsItem4 = cache2.ContainsItem(MyCacheKey4);
            Assert.IsTrue(containsItem4, "The cache was not indicated to contain item 4 when it was expected to be.");
        }

        [TestMethod]
        public void UnitTest_Cache_Load_With_Invalid_Type_In_Dictionary()
        {
            const string MyTestFileName = "MyTest3";

            // Create a cache and add an item.
            IsolatedStorageCache cache1 = new IsolatedStorageCache(MyTestFileName);
            CacheItem cacheItem = new CacheItem(new ComplexTypeForAddingToCache { PropertyOne = 1, PropertyTwo = "a" }, DateTime.Now.AddDays(1));
            cache1.AddItem("myKey", cacheItem);

            // Update the cache so that the type name is invalid.
            SetBadTypeNameInCacheXmlFile(cache1);

            // Try to load the cache from new. Should quietly fail to read, not throwing any error
            ICache cache2 = new IsolatedStorageCache(MyTestFileName);
            Assert.IsNotNull(cache2);
        }

        private static void SetBadTypeNameInCacheXmlFile(IsolatedStorageCache isolatedStorageCache)
        {
            IsolatedStorageFileStream isolatedStorageFileStreamForRead = isolatedStorageCache.GetIsolatedStorageFileStreamForRead();
            XmlDocument xmlDocument = new XmlDocument();
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(isolatedStorageFileStreamForRead);
                xmlDocument.Load(sr);
                XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
                xmlNamespaceManager.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                xmlNamespaceManager.AddNamespace("xsd", "http://www.w3.org/2001/XMLSchema");
                XmlNode stringNode = xmlDocument.SelectSingleNode("/IsolatedStorageCacheDictionary/Item/Type/string", xmlNamespaceManager);
                if (stringNode == null)
                {
                    Assert.Fail("Couldn't update the Type name in the XML.");
                }

                stringNode.InnerText = "BadTypeName";
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
                else
                {
                    isolatedStorageFileStreamForRead.Dispose();
                }
            }

            IsolatedStorageFileStream isolatedStorageFileStreamForWrite = isolatedStorageCache.GetIsolatedStorageFileStreamForWrite();
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(isolatedStorageFileStreamForWrite);
                xmlDocument.Save(sw);
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
                else
                {
                    isolatedStorageFileStreamForWrite.Dispose();
                }
            }
        }
    }
}
