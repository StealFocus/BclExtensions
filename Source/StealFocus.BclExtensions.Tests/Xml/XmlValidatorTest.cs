// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="XmlValidatorTest.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the XmlValidatorTest type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.BclExtensions.Tests.Xml
{
    using System;
    using System.Xml;
    using System.Xml.Schema;

    using BclExtensions.Xml;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for <see cref="XmlValidator"/>.
    /// </summary>
    [TestClass]
    public class XmlValidatorTest
    {
        /// <summary>
        /// Tests <see cref="XmlValidator.Validate(XmlDocument, XmlSchemaSet)"/>.
        /// </summary>
        [TestMethod]
        public void UnitTest_Validating_An_Xml_Document_Against_A_Schema()
        {
            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add(Resource.GetXmlSchema("StealFocus.BclExtensions.Tests", "StealFocus.BclExtensions.Tests.Xml.Resources.Schema.xsd"));
            XmlDocument xmlDocument = Resource.GetXmlDocument("StealFocus.BclExtensions.Tests", "StealFocus.BclExtensions.Tests.Xml.Resources.Document.xml");
            XmlValidator.Validate(xmlDocument, schemas);
        }

        /// <summary>
        /// Tests <see cref="XmlValidator.Validate(XmlDocument, XmlSchemaSet)"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UnitTest_Validating_With_Null_Parameters_Throws_ArgumentNullException()
        {
            XmlValidator.Validate(null, null);
        }
    }
}