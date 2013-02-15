// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ResourceTests.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the ResourceTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.BclExtensions.Tests
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Xml;
    using System.Xml.Schema;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public partial class ResourceTests
    {
        private const string ResourceAssemblyName = "StealFocus.BclExtensions.Tests";

        [TestMethod]
        public void UnitTest_That_Get_String_Is_Successful()
        {
            string resourceString = Resource.GetString(typeof(ResourceTests), "GoodResourceName");
            Assert.AreEqual("My string.", resourceString, "The resource string was not as expected.");
        }

        [TestMethod]
        [ExpectedException(typeof(BclExtensionsException), "No Resource String matching the key 'BadResourceName' could be found for type 'StealFocus.Core.Resource'.")]
        public void UnitTest_That_Get_String_Throws_Expected_Exception_When_Unsuccessful()
        {
            Resource.GetString(typeof(ResourceTests), "BadResourceName");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Parameter may not be null.")]
        public void UnitTest_That_Get_String_Throws_Expected_Exception_When_Using_Null_Type()
        {
            Resource.GetString(null, "SomeResourceName");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Parameter may not be null.")]
        public void UnitTest_That_Get_String_Throws_Expected_Exception_When_Using_Null_Key()
        {
            Resource.GetString(this, null);
        }

        [TestMethod]
        public void UnitTest_That_Get_XmlDocument_Is_Successful()
        {
            XmlDocument myXmlDocument = Resource.GetXmlDocument(ResourceAssemblyName, ResouceNames.MyXmlDocumentResource);
            Assert.IsNotNull(myXmlDocument, "The XML Document was null when it was not expected to be.");
        }

        [TestMethod]
        [ExpectedException(typeof(BclExtensionsException), "The requested Assembly 'BadAssemblyName' holding the Resource could not be found.")]
        public void UnitTest_That_Get_XmlDocument_Throws_Expected_Exception_With_Bad_Assembly_Name()
        {
            Resource.GetXmlDocument("BadAssemblyName", "ResourceNameIsIrrelevant");
        }

        [TestMethod]
        [ExpectedException(typeof(BclExtensionsException), "The schema resource 'BadResourceName' was not found in the assembly 'StealFocus.Core'.")]
        public void UnitTest_That_Get_XmlDocument_Throws_Expected_Exception_With_Bad_Resource_Name()
        {
            Resource.GetXmlDocument(ResourceAssemblyName, "BadResourceName");
        }

        [TestMethod]
        public void UnitTest_That_Get_XmlSchema_Is_Successful()
        {
            XmlSchema schema = Resource.GetXmlSchema(ResourceAssemblyName, "StealFocus.BclExtensions.Tests.Resources.MyXsdResource.xsd");
            Assert.IsNotNull(schema, "The Schema was null where it was not expected to be.");
        }

        [TestMethod]
        [ExpectedException(typeof(BclExtensionsException))]
        public void UnitTest_That_Get_XmlSchema_Throws_Expected_Exception_With_Bad_Resource_Name()
        {
            Resource.GetXmlSchema(ResourceAssemblyName, "BadResourceName");
        }

        [TestMethod]
        public void UnitTest_That_Get_XmlSchema_Throws_Expected_Exception_With_Badly_Formed_Resource_Content()
        {
            try
            {
                Resource.GetXmlSchema(ResourceAssemblyName, "StealFocus.BclExtensions.Tests.Resources.MyBadlyFormedXsdResource.xsd");
            }
            catch (BclExtensionsException e)
            {
                Assert.IsTrue(e.InnerException.GetType() == typeof(XmlException), "The inner exception was not as expected.");
            }
        }

        [TestMethod]
        public void UnitTest_That_Get_XmlSchema_Throws_Expected_Exception_With_Invalid_Resource_Content()
        {
            try
            {
                Resource.GetXmlSchema(ResourceAssemblyName, "StealFocus.BclExtensions.Tests.Resources.MyInvalidXsdResource.xsd");
            }
            catch (BclExtensionsException e)
            {
                Assert.IsTrue(e.InnerException.GetType() == typeof(XmlSchemaException), "The inner exception was not as expected.");
            }
        }

        [TestMethod]
        public void UnitTest_That_Get_File_And_Write_To_Path_Is_Successful()
        {
            const string FileOutputPath = "SomeFile.txt";
            Resource.GetFileAndWriteToPath(ResourceAssemblyName, ResouceNames.SomeFileResource, FileOutputPath);
            Assert.IsTrue(File.Exists(FileOutputPath), "The file did not exist where it was expected to.");
        }

        [TestMethod]
        public void UnitTest_That_Get_Image_Is_Successful()
        {
            Bitmap image = Resource.GetImage(ResourceAssemblyName, ResouceNames.TestImage);
            Assert.IsNotNull(image, "The image returned was null where it was not expected to be.");
        }

        private struct ResouceNames
        {
            public const string MyXmlDocumentResource = "StealFocus.BclExtensions.Tests.Resources.MyXmlDocumentResource.xml";

            public const string SomeFileResource = "StealFocus.BclExtensions.Tests.Resources.SomeFile.txt";

            public const string TestImage = "StealFocus.BclExtensions.Tests.Resources.TestImage.bmp";
        }
    }
}