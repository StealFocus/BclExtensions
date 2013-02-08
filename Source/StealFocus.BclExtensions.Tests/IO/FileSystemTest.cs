// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="FileSystemTest.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the FileSystemTest type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.BclExtensions.Tests.IO
{
    using System;
    using System.Reflection;

    using BclExtensions.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for <see cref="FileSystem"/>.
    /// </summary>
    [TestClass]
    public class FileSystemTest
    {
        /// <summary>
        /// Holds the name of the resource assembly.
        /// </summary>
        private const string ResourceAssemblyName = "StealFocus.BclExtensions.Tests";

        /// <summary>
        /// Tests <see cref="FileSystem.ComputeHash(string)"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UnitTest_That_Compute_Hash_With_Null_File_Path_Throws_ArgumentNullException()
        {
            FileSystem.ComputeHash(null);
        }

        /// <summary>
        /// Tests <see cref="FileSystem.ComputeHash(string)"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(BclExtensionsException), @"The provided file path of 'C:\Path\InvalidFile.txt' for parameter 'pathToFile' was not valid.")]
        public void UnitTest_That_Compute_Hash_With_Invalid_File_Path_Throws_BclExtensionsException()
        {
            FileSystem.ComputeHash(@"C:\Path\InvalidFile.txt");
        }

        /// <summary>
        /// Tests <see cref="FileSystem.CompareHash(string, string)"/>.
        /// </summary>
        [TestMethod]
        public void IntegrationTest_That_Two_Identical_Files_Are_Reported_As_The_Same()
        {
            Resource.GetFileAndWriteToPath(ResourceAssemblyName, ResourcePaths.SomeFile, FilePaths.SomeFilePath);
            Resource.GetFileAndWriteToPath(ResourceAssemblyName, ResourcePaths.FileTheSameAsSomeFile, FilePaths.FileTheSameAsSomeFilePath);
            Assert.IsTrue(FileSystem.CompareHash(FilePaths.SomeFilePath, FilePaths.FileTheSameAsSomeFilePath), "The files were reported as different when they were not expected to be.");
        }

        /// <summary>
        /// Tests <see cref="FileSystem.CompareHash(string, string)"/>.
        /// </summary>
        [TestMethod]
        public void IntegrationTest_That_Two_Different_Files_Are_Not_Reported_As_The_Same()
        {
            Resource.GetFileAndWriteToPath(ResourceAssemblyName, ResourcePaths.SomeFile, FilePaths.SomeFilePath);
            Resource.GetFileAndWriteToPath(ResourceAssemblyName, ResourcePaths.FileDifferentToSomeFile, FilePaths.FileDifferentToSomeFilePath);
            Assert.IsFalse(FileSystem.CompareHash(FilePaths.SomeFilePath, FilePaths.FileDifferentToSomeFilePath), "The files were reported as the same when they were not expected to be.");
        }

        /// <summary>
        /// Tests <see cref="FileSystem.IsAssembly(string)"/>.
        /// </summary>
        [TestMethod]
        public void IntegrationTest_That_An_Assembly_Is_Reported_As_An_Assembly()
        {
            string validAssemblyPath = Assembly.GetExecutingAssembly().Location;
            bool isAssembly = FileSystem.IsAssembly(validAssemblyPath);
            Assert.IsTrue(isAssembly, "The Assembly was reported as not being an assembly when it was expected to be.");
        }

        /// <summary>
        /// Tests <see cref="FileSystem.IsAssembly(string)"/>.
        /// </summary>
        [TestMethod]
        public void IntegrationTest_That_A_Non_Assembly_Is_Not_Reported_As_An_Assembly()
        {
            const string InvalidAssemblyPath = "StealFocus.BclExtensions.Tests.dll.config";
            bool isAssembly = FileSystem.IsAssembly(InvalidAssemblyPath);
            Assert.IsFalse(isAssembly, "The file was reported as being an assembly when it was not expected to be.");
        }

        /// <summary>
        /// Tests <see cref="FileSystem.CopyAccessControlList(string, string)"/>.
        /// </summary>
        [TestMethod]
        public void IntegrationTest_That_Acls_Will_Copy()
        {
            FileSystem.CopyAccessControlList("StealFocus.BclExtensions.dll", "StealFocus.BclExtensions.Tests.dll.config");
        }

        /// <summary>
        /// Clean up after the tests.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            System.IO.File.Delete("SomeFile.txt");
            System.IO.File.Delete("FileTheSameAsSomeFile.txt");
            System.IO.File.Delete("FileDifferentToSomeFile.txt");
        }

        /// <summary>
        /// Holds the file paths.
        /// </summary>
        private struct FilePaths
        {
            /// <summary>
            /// Path to SomeFile.txt.
            /// </summary>
            public const string SomeFilePath = @"SomeFile.txt";

            /// <summary>
            /// Path to FileTheSameAsSomeFile.txt.
            /// </summary>
            public const string FileTheSameAsSomeFilePath = @"FileTheSameAsSomeFile.txt";

            /// <summary>
            /// Path to FileDifferentToSomeFile.txt.
            /// </summary>
            public const string FileDifferentToSomeFilePath = @"FileDifferentToSomeFile.txt";
        }

        /// <summary>
        /// Holds the resource paths.
        /// </summary>
        private struct ResourcePaths
        {
            /// <summary>
            /// Path to SomeFile.txt.
            /// </summary>
            public const string SomeFile = @"StealFocus.BclExtensions.Tests.IO.Resources.SomeFile.txt";

            /// <summary>
            /// Path to FileTheSameAsSomeFile.txt.
            /// </summary>
            public const string FileTheSameAsSomeFile = @"StealFocus.BclExtensions.Tests.IO.Resources.FileTheSameAsSomeFile.txt";

            /// <summary>
            /// Path to FileDifferentToSomeFile.txt.
            /// </summary>
            public const string FileDifferentToSomeFile = @"StealFocus.BclExtensions.Tests.IO.Resources.FileDifferentToSomeFile.txt";
        }
    }
}