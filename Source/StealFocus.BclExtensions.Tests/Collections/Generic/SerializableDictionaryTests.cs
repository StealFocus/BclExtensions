// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="SerializableDictionaryTests.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the DictionaryTest type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.BclExtensions.Tests.Collections.Generic
{
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    using BclExtensions.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// SerializableDictionaryTests Class.
    /// </summary>
    [TestClass]
    public class SerializableDictionaryTests
    {
        /// <summary>
        /// Tests serialization.
        /// </summary>
        [TestMethod]
        public void UnitTest_That_Dictionary_Can_Serialize()
        {
            SerializableDictionary<string, string> dictionary = new SerializableDictionary<string, string>();
            dictionary.Add("myKey", "myValue");
            XmlSerializer xmlSerializer = new XmlSerializer(dictionary.GetType());
            string xml;
            MemoryStream memoryStream = null;
            XmlTextWriter xmlTextWriter = null;
            StreamReader streamReader = null;
            try
            {
                memoryStream = new MemoryStream();
                xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                streamReader = new StreamReader(memoryStream, Encoding.UTF8);
                xmlSerializer.Serialize(xmlTextWriter, dictionary);
                memoryStream.Position = 0;
                xml = streamReader.ReadToEnd();
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Dispose();
                }
                else if (xmlTextWriter != null)
                {
                    xmlTextWriter.Close();
                }
                else if (memoryStream != null)
                {
                    memoryStream.Dispose();
                }
            }

            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?><Dictionary><item><key><string>myKey</string></key><value><string>myValue</string></value></item></Dictionary>", xml, "The XML representing the serialized dictionary was not as expected.");
        }

        /// <summary>
        /// Tests deserialization.
        /// </summary>
        [TestMethod]
        public void UnitTest_That_Dictionary_Can_Deserialize()
        {
            const string Xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Dictionary><item><key><string>myKey</string></key><value><string>myValue</string></value></item></Dictionary>";
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SerializableDictionary<string, string>));
            SerializableDictionary<string, string> deserializedDictionary;
            using (StringReader stringReader = new StringReader(Xml))
            {
                deserializedDictionary = (SerializableDictionary<string, string>)xmlSerializer.Deserialize(stringReader);
            }

            Assert.IsNotNull(deserializedDictionary, "The deserialized dictionary was null where it was not expected to be.");
            Assert.IsNotNull(deserializedDictionary["myKey"], "The expected value in the dictionary was not found.");
            Assert.AreEqual("myValue", deserializedDictionary["myKey"], "The value was not as expected.");
        }
    }
}
