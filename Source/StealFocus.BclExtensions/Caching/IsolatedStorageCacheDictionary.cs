// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IsolatedStorageCacheDictionary.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the IsolatedStorageCacheDictionary type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.BclExtensions.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    [Serializable]
    public class IsolatedStorageCacheDictionary : Dictionary<string, CacheItem>, IXmlSerializable
    {
        public IsolatedStorageCacheDictionary() 
        {
        }

        protected IsolatedStorageCacheDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            XmlSerializer stringSerializer = new XmlSerializer(typeof(string));
            XmlSerializer dateTimeSerializer = new XmlSerializer(typeof(DateTime));
            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();
            if (wasEmpty)
            {
                return;
            }

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement("Item");

                reader.ReadStartElement("Key");
                string key = (string)stringSerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("Type");
                string assemblyQualifiedName = (string)stringSerializer.Deserialize(reader);
                reader.ReadEndElement();

                // Try to read the type, if the assembly does not exist (i.e. a new version has been installed) catch the exception
                // quietly because the assembly cannot be found
                Type type;
                try
                {
                    // the true argument tell the method to always throw an exception if there is a problem
                    type = Type.GetType(assemblyQualifiedName, true);
                }
                catch
                {
                    // This could be anything:
                    //  - The XML value is corrupt or incorrect somehow.
                    //  - The type is no longer available to the AppDomain - maybe the application was upgraded to a new version and the old assembly versions can't be loaded.
                    // So, skip this entry in the dictionary entirely, something has gone wrong. This is just a cache, it doesn't really matter. Write out a warning so the problem can be picked up through investigation.
                    string warningMessage = string.Format(CultureInfo.CurrentCulture, "Could not determine the Type for assembly qualified name '{0}' so could not de-serialise from the cache. This item will not be restored to the cache.", assemblyQualifiedName);
                    System.Diagnostics.Debug.WriteLine(warningMessage);

                    // Read and skip any value item
                    reader.ReadStartElement("Value");
                    reader.Skip();
                    reader.ReadEndElement();

                    // Read and skip any expiry item
                    reader.ReadStartElement("Expiry");
                    reader.Skip();
                    reader.ReadEndElement();

                    // Read the closing tag for the item
                    reader.ReadEndElement();

                    // Go back to the start of the loop looking at the next item in the cache (which could be valid)
                    continue;
                }

                reader.ReadStartElement("Value");
                XmlSerializer valueSerializer = new XmlSerializer(type);
                object value = valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("Expiry");
                DateTime expiry = (DateTime)dateTimeSerializer.Deserialize(reader);
                reader.ReadEndElement();

                CacheItem cacheItem = new CacheItem(value, expiry);
                this.Add(key, cacheItem);

                reader.ReadEndElement();
            }

            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            XmlSerializer stringSerializer = new XmlSerializer(typeof(string));
            XmlSerializer dateTimeSerializer = new XmlSerializer(typeof(DateTime));
            foreach (string key in this.Keys)
            {
                CacheItem value = this[key];
                Type cacheItemValueType = value.ItemValue.GetType();
                if (cacheItemValueType.AssemblyQualifiedName == null)
                {
                    string message = string.Format(CultureInfo.CurrentCulture, "Could not determine the Assembly Qualified Name of type '{0}' so could not serialize and add to the cache.", cacheItemValueType.FullName);
                    throw new CachingException(message);
                }
                
                writer.WriteStartElement("Item");

                writer.WriteStartElement("Key");
                stringSerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("Type");
                stringSerializer.Serialize(writer, cacheItemValueType.AssemblyQualifiedName);
                writer.WriteEndElement();

                writer.WriteStartElement("Value");
                XmlSerializer valueSerializer = new XmlSerializer(cacheItemValueType);
                valueSerializer.Serialize(writer, value.ItemValue);
                writer.WriteEndElement();

                writer.WriteStartElement("Expiry");
                dateTimeSerializer.Serialize(writer, value.Expiration);
                writer.WriteEndElement();
                
                writer.WriteEndElement();
            }
        }
    }
}
