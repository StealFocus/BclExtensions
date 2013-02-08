// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleDataContractSerializer.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the SimpleDataContractSerializer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.BclExtensions.Xml.Serialization
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Xml;

    public static class SimpleDataContractSerializer<T>
    {
        /// <summary>
        /// Serializes to xml using DataContractSerializer
        /// </summary>
        /// <typeparam name="T">Serialization target type</typeparam>
        /// <param name="target">Serialization target</param>
        /// <returns>Xml from target</returns>
        public static XmlDocument Serialize(T target)
        {
            XmlDocument xmlDocument = new XmlDocument();
            StringBuilder stringBuilder = new StringBuilder();
            DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(T));
            using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder))
            {
                dataContractSerializer.WriteObject(xmlWriter, target);
                xmlWriter.Flush();
                xmlDocument.LoadXml(stringBuilder.ToString());
            }

            return xmlDocument;
        }

        /// <summary>
        /// Calls DataContractSerializer.ReadObject method 
        /// </summary>
        /// <typeparam name="T">Deserialization target type</typeparam>
        /// <param name="xmlDocument">The input xml</param>
        /// <returns>The deserialized type</returns>
        public static T Deserialize(XmlDocument xmlDocument)
        {
            if (xmlDocument == null)
            {
                throw new ArgumentNullException("xmlDocument");
            }

            T t;
            StringReader stringReader = new StringReader(xmlDocument.OuterXml);
            XmlReader xmlReader = null;
            try
            {
                xmlReader = XmlReader.Create(stringReader);
                xmlReader.MoveToContent();
                DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(T));
                t = (T)dataContractSerializer.ReadObject(xmlReader);
            }
            finally
            {
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
                else
                {
                    stringReader.Close();
                }
            }

            return t;
        }
    }
}
