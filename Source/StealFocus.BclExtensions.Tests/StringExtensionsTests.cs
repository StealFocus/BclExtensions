namespace StealFocus.BclExtensions.Tests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void UnitTest_That_Generated_Guids_Are_The_Same_For_The_Same_String_In_The_Same_Namespace()
        {
            Guid namespaceId = Guid.NewGuid();
            const string Test = "test";
            Guid guid1 = Test.CreateGuid(namespaceId);
            Guid guid2 = Test.CreateGuid(namespaceId);
            Assert.IsTrue(guid1 == guid2, "The GUIDs are not the same when they are expected to be so.");
        }

        [TestMethod]
        public void UnitTest_That_Generated_Guids_Are_Not_The_Same_For_The_Same_String_In_Different_Namespaces()
        {
            Guid namespaceId1 = Guid.NewGuid();
            Guid namespaceId2 = Guid.NewGuid();
            const string Test = "test";
            Guid guid1 = Test.CreateGuid(namespaceId1);
            Guid guid2 = Test.CreateGuid(namespaceId2);
            Assert.IsTrue(guid1 != guid2, "The GUIDs are the same when they are expected not to be so.");
        }

        [TestMethod]
        public void UnitTest_That_Generated_Guids_Are_Not_The_Same_For_Different_Strings_In_The_Same_Namespaces()
        {
            Guid namespaceId = Guid.NewGuid();
            const string Test1 = "abc";
            const string Test2 = "def";
            Guid guid1 = Test1.CreateGuid(namespaceId);
            Guid guid2 = Test2.CreateGuid(namespaceId);
            Assert.IsTrue(guid1 != guid2, "The GUIDs are the same when they are expected not to be so.");
        }
    }
}
