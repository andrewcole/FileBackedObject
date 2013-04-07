// <copyright file="XmlFileBackedObject.cs" company="Illallangi Enterprises">Copyright © 2012 Illallangi Enterprises</copyright>

using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Illallangi
{
    using System;

    /// <summary>
    /// An class that allows serializing to and from a XML file.
    /// </summary>
    /// <typeparam name="T">The type to back.</typeparam>
    public abstract class XmlFileBackedObject<T> : IFileBackedObject<T> where T : XmlFileBackedObject<T>
    {
        #region Fields

        /// <summary>
        /// The current value of the FileBackedSource property.
        /// </summary>
        private string currentFileBackedSource;

        #endregion
        
        #region Properties

        /// <summary>
        /// Gets a value indicating whether the object was loaded from a file.
        /// </summary>
        /// <value>
        /// A value indicating whether the object was loaded from a file.
        /// </value>
        protected bool FileBacked
        {
            get
            {
                return !string.IsNullOrEmpty(this.currentFileBackedSource);
            }
        }

        /// <summary>
        /// Gets the path of the file the object was loaded from, or null if the object is new or created from a string.
        /// </summary>
        /// <value>
        /// The path of the file the object was loaded from, or null if the object is new or created from a string.
        /// </value>
        protected string FileBackedSource
        {
            get
            {
                return this.currentFileBackedSource;
            }
        }

        #endregion

        #region Methods

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
        /// Deserializes a string to a T.
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

        #region Non-Static Methods

        /// <summary>
        /// Serializes this T to its backing file.
        /// </summary>
        /// <returns>The object being serialized.</returns>
        /// <exception cref="ArgumentException">
        /// Throws if object is not backed by a file.
        /// </exception>
        public virtual T ToFile()
        {
            if (!this.FileBacked)
            {
                throw new ArgumentException("Attempted to save to original file when object was not file backed.");
            }

            return this.ToFile(this.FileBackedSource);
        }

        /// <summary>
        /// Serializes this T to a file.
        /// </summary>
        /// <param name="fileName">The filename to serialize to.</param>
        /// <returns>The object being serialized.</returns>
        public virtual T ToFile(string fileName)
        {
            File.WriteAllText(fileName, this.ToString());
            return this.SetFileBackedSource(fileName);
        }

        /// <summary>
        /// Serializes this T to a string.
        /// </summary>
        /// <returns>A string serialization of this T.</returns>
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

        /// <summary>
        /// Sets the source of the object.
        /// </summary>
        /// <param name="fileBackedSource">
        /// The source of the object, either a filename if the object is backed by a file, or null or empty otherwise.
        /// </param>
        /// <returns>
        /// This object<see cref="T"/>.
        /// </returns>
        private T SetFileBackedSource(string fileBackedSource)
        {
            if (string.IsNullOrEmpty(fileBackedSource) || !File.Exists(fileBackedSource))
            {
                this.currentFileBackedSource = null;
            }
            else
            {
                this.currentFileBackedSource = fileBackedSource;
            }

            return (T)this;
        }

        #endregion

        #endregion
    }
}
