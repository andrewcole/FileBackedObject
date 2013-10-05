// <copyright file="XmlFileBackedObject.cs" company="Illallangi Enterprises">Copyright © 2012 Illallangi Enterprises</copyright>

using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Illallangi
{
    /// <summary>
    /// An class that allows serializing to and from a XML file.
    /// </summary>
    /// <typeparam name="T">The type to back.</typeparam>
    public class XmlFileBackedObject<T> : FileBackedObject<T> where T : XmlFileBackedObject<T>
    {
        #region Methods

        #region Non-Static Methods

        /// <summary>
        /// Serializes this T to a XML string.
        /// </summary>
        /// <returns>A XML string serialization of this T.</returns>
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            var xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add(string.Empty, string.Empty);

            var xmlSerializer = new XmlSerializer(typeof(T));

            var xmlWriterSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true
            };

            using (var xmlWriter = XmlWriter.Create(stringBuilder, xmlWriterSettings))
            {
                xmlSerializer.Serialize(xmlWriter, this, xmlSerializerNamespaces);
                xmlWriter.Close();
                return stringBuilder.ToString();
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Deserializes the specified XML file to a T.
        /// </summary>
        /// <param name="fileName">The file to deserialize.</param>
        /// <returns>A T deserialized from the specified XML file.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Required to allow implementations of this class to be deserialized.")]
        public static T FromFile(string fileName)
        {
            return FromString(File.ReadAllText(fileName)).SetFileBackedSource(fileName);
        }

        /// <summary>
        /// Deserializes a JSON string to a T.
        /// </summary>
        /// <param name="input">The string to deserialize.</param>
        /// <returns>A T deserialized from the specified string.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Required to allow implementations of this class to be deserialized.")]
        public static T FromString(string input)
        {
            var ser = new XmlSerializer(typeof(T));
            using (var sr = new StringReader(input))
            {
                var ret = (T)ser.Deserialize(sr);
                return ret;
            }
        }

        #endregion

        #endregion
    }
}
