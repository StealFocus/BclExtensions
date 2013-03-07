﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IsolatedStorageCache.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the IsolatedStorageCache type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.BclExtensions.Caching
{
    using System;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Xml.Serialization;

    public class IsolatedStorageCache : ICache
    {
        private const string AllFilesSearchPattern = "*";

        private static readonly string CacheFileNamePrefix = typeof(IsolatedStorageCache).FullName + "_";

        private static readonly object SyncRoot = new object();

        private readonly string cacheFileName;

        private IsolatedStorageCacheDictionary cachedItemDictionary;

        public IsolatedStorageCache() : this(GetRandomCacheFileName())
        {
        }

        public IsolatedStorageCache(string cacheFileName)
        {
            this.cacheFileName = CacheFileNamePrefix + cacheFileName;
            this.Load();
        }

        public static void ClearAll()
        {
            using (IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForAssembly())
            {
                string[] fileNames = isolatedStorageFile.GetFileNames(AllFilesSearchPattern);
                foreach (string fileName in fileNames)
                {
                    if (fileName.StartsWith(CacheFileNamePrefix, StringComparison.OrdinalIgnoreCase))
                    {
                        isolatedStorageFile.DeleteFile(fileName);
                        break;
                    }
                }
            }
        }

        public void AddItem(string cacheKey, CacheItem cacheItem)
        {
            lock (SyncRoot)
            {
                this.cachedItemDictionary.Add(cacheKey, cacheItem);
                this.Save();
            }
        }

        public bool ContainsItem(string cacheKey)
        {
            lock (SyncRoot)
            {
                bool cacheContainsItem = this.cachedItemDictionary.ContainsKey(cacheKey);
                if (!cacheContainsItem)
                {
                    return false;
                }

                bool cacheItemHasExpired = HasCacheItemExpired(this.cachedItemDictionary[cacheKey]);
                if (cacheItemHasExpired)
                {
                    this.Remove(cacheKey);
                    this.Save();
                    return false;
                }

                return this.cachedItemDictionary.ContainsKey(cacheKey);
            }
        }

        public object GetItem(string cacheKey)
        {
            lock (SyncRoot)
            {
                bool cacheContainsItem = this.cachedItemDictionary.ContainsKey(cacheKey);
                if (!cacheContainsItem)
                {
                    return null;
                }

                bool cacheItemHasExpired = HasCacheItemExpired(this.cachedItemDictionary[cacheKey]);
                if (cacheItemHasExpired)
                {
                    this.Remove(cacheKey);
                    this.Save();
                    return null;
                }

                return this.cachedItemDictionary[cacheKey].ItemValue;
            }
        }

        public void Remove(string cacheKey)
        {
            lock (SyncRoot)
            {
                this.cachedItemDictionary.Remove(cacheKey);
                this.Save();
            }
        }

        public void Clear()
        {
            using (IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForAssembly())
            {
                string[] fileNames = isolatedStorageFile.GetFileNames(this.cacheFileName);
                foreach (string fileName in fileNames)
                {
                    if (fileName == this.cacheFileName)
                    {
                        isolatedStorageFile.DeleteFile(this.cacheFileName);
                        break;
                    }
                }
            }

            this.cachedItemDictionary = new IsolatedStorageCacheDictionary();
        }

        internal IsolatedStorageFileStream GetIsolatedStorageFileStreamForRead()
        {
            IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForAssembly();
            return new IsolatedStorageFileStream(this.cacheFileName, FileMode.Open, FileAccess.Read, FileShare.Read, isolatedStorageFile);
        }

        internal IsolatedStorageFileStream GetIsolatedStorageFileStreamForWrite()
        {
            IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForAssembly();
            return new IsolatedStorageFileStream(this.cacheFileName, FileMode.Create, FileAccess.Write, FileShare.None, isolatedStorageFile);
        }

        private static string GetRandomCacheFileName()
        {
            return Guid.NewGuid().ToString("D");
        }

        private static bool HasCacheItemExpired(CacheItem cacheItem)
        {
            return cacheItem.Expiration < DateTime.Now;
        }

        private bool CacheFileExists()
        {
            using (IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForAssembly())
            {
                string[] fileNames = isolatedStorageFile.GetFileNames(this.cacheFileName);
                foreach (string fileName in fileNames)
                {
                    if (fileName == this.cacheFileName)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void Load()
        {
            lock (SyncRoot)
            {
                if (this.CacheFileExists())
                {
                    IsolatedStorageFile isolatedStorageFile = null;
                    IsolatedStorageFileStream isolatedStorageFileStream = null;
                    StreamReader streamReader = null;
                    try
                    {
                        isolatedStorageFile = IsolatedStorageFile.GetUserStoreForAssembly();
                        isolatedStorageFileStream = new IsolatedStorageFileStream(this.cacheFileName, FileMode.Open, FileAccess.Read, FileShare.Read, isolatedStorageFile);
                        XmlSerializer xs = new XmlSerializer(typeof(IsolatedStorageCacheDictionary));
                        streamReader = new StreamReader(isolatedStorageFileStream);
                        this.cachedItemDictionary = (IsolatedStorageCacheDictionary)xs.Deserialize(streamReader);
                    }
                    finally
                    {
                        if (streamReader != null)
                        {
                            streamReader.Close();
                        }
                        else if (isolatedStorageFileStream != null)
                        {
                            isolatedStorageFileStream.Close();
                        }
                        else if (isolatedStorageFile != null)
                        {
                            isolatedStorageFile.Close();
                        }
                    }
                }
                else
                {
                    this.cachedItemDictionary = new IsolatedStorageCacheDictionary();
                }
            }
        }

        private void Save()
        {
            lock (SyncRoot)
            {
                IsolatedStorageFile isolatedStorageFile = null;
                IsolatedStorageFileStream isolatedStorageFileStream = null;
                StreamWriter streamWriter = null;
                try
                {
                    isolatedStorageFile = IsolatedStorageFile.GetUserStoreForAssembly();
                    isolatedStorageFileStream = new IsolatedStorageFileStream(this.cacheFileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, isolatedStorageFile);
                    XmlSerializer xs = new XmlSerializer(typeof(IsolatedStorageCacheDictionary));
                    streamWriter = new StreamWriter(isolatedStorageFileStream);
                    xs.Serialize(streamWriter, this.cachedItemDictionary);
                }
                finally
                {
                    if (streamWriter != null)
                    {
                        streamWriter.Close();
                    }
                    else if (isolatedStorageFileStream != null)
                    {
                        isolatedStorageFileStream.Close();
                    }
                    else if (isolatedStorageFile != null)
                    {
                        isolatedStorageFile.Close();
                    }
                }
            }
        }
    }
}
