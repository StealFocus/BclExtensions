namespace StealFocus.BclExtensions.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;

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

        [TestMethod]
        public void UnitTest_That_Strings_Of_Up_To_Five_Characters_In_Length_Never_Generate_Duplicates()
        {
            const int RequiredLength = 5;
            double expectedNumberOfTestValues = 0;
            for (int i = 1; i <= RequiredLength; i++)
            {
                expectedNumberOfTestValues += Math.Pow(26, i);
            }

            Dictionary<Guid, string> testSet = new Dictionary<Guid, string>(Convert.ToInt32(expectedNumberOfTestValues));
            TestStringCombinations(new StringBuilder(), RequiredLength, testSet, Guid.NewGuid());
            Assert.AreEqual(expectedNumberOfTestValues, testSet.Count);
        }

        private static void TestStringCombinations(StringBuilder currentString, int maxLength, Dictionary<Guid, string> testSet, Guid guidNamespace)
        {
            if (currentString.Length == maxLength)
            {
                return;
            }

            char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            foreach (char c in chars)
            {
                currentString.Append(c);
                string s = currentString.ToString();
                Guid newKey = s.CreateGuid(guidNamespace);
                Assert.IsFalse(testSet.ContainsKey(newKey), "The test set already contained the key when it was not expected to, this means a duplicate GUID has been generated.");
                testSet.Add(newKey, null);
                TestStringCombinations(currentString, maxLength, testSet, guidNamespace);
                currentString.Remove(currentString.Length - 1, 1);
            }
        }
    }
}
