// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="SimpleXmlSerializerTest.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the SimpleXmlSerializerTest type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.BclExtensions.Tests.Xml.Serialization
{
    using BclExtensions.Xml.Serialization;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for SimpleXmlSerializer.
    /// </summary>
    [TestClass]
    public class SimpleXmlSerializerTest
    {
        /// <summary>
        /// Tests serialize.
        /// </summary>
        [TestMethod]
        public void UnitTest_That_Something_Will_Serialize()
        {
            SerializableWidget serializableWidget = new SerializableWidget();
            serializableWidget.MyProperty = "SomeValue";
            string xml = SimpleXmlSerializer<SerializableWidget>.Serialize(serializableWidget);
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?><SerializableWidget xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><MyProperty>SomeValue</MyProperty></SerializableWidget>", xml, "The XML produced from the serialization process was not as expected.");
        }

        /// <summary>
        /// Tests deserialize with namespace.
        /// </summary>
        [TestMethod]
        public void UnitTest_That_Something_Will_Serialize_With_Xml_Namespace()
        {
            SerializableWidget serializableWidget = new SerializableWidget();
            serializableWidget.MyProperty = "SomeValue";
            string xml = SimpleXmlSerializer<SerializableWidget>.Serialize(serializableWidget, "http://www.MyCompany.com/Widgets");
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?><SerializableWidget xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.MyCompany.com/Widgets\"><MyProperty>SomeValue</MyProperty></SerializableWidget>", xml, "The XML produced from the serialization process was not as expected.");
        }

        /// <summary>
        /// Tests deserialize.
        /// </summary>
        [TestMethod]
        public void UnitTest_That_Something_Will_Deserialize()
        {
            string xml = Resource.GetXmlDocument("StealFocus.BclExtensions.Tests", ResouceNames.SerializableWidgetXml).OuterXml;
            SerializableWidget widget = SimpleXmlSerializer<SerializableWidget>.Deserialize(xml);
            Assert.IsNotNull(widget, "The object returned from the deserialization process was null when it was not expected to be.");
            Assert.AreEqual("MyValue", widget.MyProperty, "The value of the property after deserialization was not as expected.");
        }

        /// <summary>
        /// Tests deserialize with namespace.
        /// </summary>
        [TestMethod]
        public void UnitTest_That_Something_Will_Deserialize_With_Xml_Namespace()
        {
            string xml = Resource.GetXmlDocument("StealFocus.BclExtensions.Tests", ResouceNames.SerializableWidgetWithXmlNamespaceXml).OuterXml;
            SerializableWidget widget = SimpleXmlSerializer<SerializableWidget>.Deserialize(xml, "http://www.MyCompany.com/Widgets");
            Assert.IsNotNull(widget, "The object returned from the deserialization process was null when it was not expected to be.");
            Assert.AreEqual("MyValue", widget.MyProperty, "The value of the property after deserialization was not as expected.");
        }

        /// <summary>
        /// Holds the resource names.
        /// </summary>
        private struct ResouceNames
        {
            /// <summary>
            /// Holds the SerializableWidget resource name.
            /// </summary>
            public const string SerializableWidgetXml = "StealFocus.BclExtensions.Tests.Xml.Serialization.Resources.SerializableWidget.xml";

            /// <summary>
            /// Holds the SerializableWidgetWithXmlNamespace resource name.
            /// </summary>
            public const string SerializableWidgetWithXmlNamespaceXml = "StealFocus.BclExtensions.Tests.Xml.Serialization.Resources.SerializableWidgetWithXmlNamespace.xml";
        }
    }
}