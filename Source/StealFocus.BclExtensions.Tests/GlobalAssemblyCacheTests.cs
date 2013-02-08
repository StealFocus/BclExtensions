// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheTests.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the GlobalAssemblyCacheTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.BclExtensions.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GlobalAssemblyCacheTests
    {
        [TestMethod]
        public void UnitTest_That_Assemblies_Are_Found_In_The_Gac()
        {
            GlobalAssemblyCacheItem[] gacAssemblies = GlobalAssemblyCache.GetAssemblyList(GlobalAssemblyCacheCategoryTypes.Gac);
            Assert.IsTrue(gacAssemblies.Length > 0, "No assemblies were returned in the list.");
        }
    }
}
