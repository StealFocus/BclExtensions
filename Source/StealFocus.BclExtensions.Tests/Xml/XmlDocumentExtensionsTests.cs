// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlDocumentExtensionsTests.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the XmlDocumentExtensionsTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.BclExtensions.Tests.Xml
{
    using System.Xml;

    using BclExtensions.Xml;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for <see cref="XmlDocumentExtensions"/>.
    /// </summary>
    [TestClass]
    public class XmlDocumentExtensionsTests
    {
        /// <summary>
        /// Holds the expected result.
        /// </summary>
        private const string TestAddElementResult = "<MyDocumentElement xmlns=\"http://schemas.acme.com/PetShop\"><MyChildElement1 myAttrib=\"myAttribValue\">MyChildElementValue</MyChildElement1><MyChildElement2 /><MyChildElement3 /><MyChildElement4 /><MyChildElement2 /></MyDocumentElement>";

        /// <summary>
        /// Tests something.
        /// </summary>
        [TestMethod]
        public void UnitTest_That_An_Element_Can_Be_Added()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument
                .AddElement("MyDocumentElement", "http://schemas.acme.com/PetShop")
                    .AddElement("MyChildElement1")
                        .AddAttribute("myAttrib", "myAttribValue")
                        .AddValue("MyChildElementValue")
                        .Parent()
                    .AddElement("MyChildElement2")
                        .Parent()
                    .AddElement("MyChildElement2")
                    .AddElementBefore("MyChildElement3")
                    .AddElementAfter("MyChildElement4");
            Assert.AreEqual(TestAddElementResult, xmlDocument.OuterXml);
            XmlElement childElement = xmlDocument.DocumentElement.Child("MyChildElement2");
            Assert.IsNotNull(childElement, "The child returned was null when it was not expected to be.");
        }
    }
}
