using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Illallangi
{
    public abstract class FileBackedObject<T> : IFileBackedObject<T> where T : FileBackedObject<T>
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
            this.SetFileBackedSource(fileName);
            File.WriteAllText(fileName, this.ToString());
            return (T)this;
        }

        /// <summary>
        /// Serializes this T to a string.
        /// </summary>
        /// <returns>A string serialization of this T.</returns>
        public abstract override string ToString();

        /// <summary>
        /// Sets the source of the object.
        /// </summary>
        /// <param name="fileBackedSource">
        /// The source of the object, either a filename if the object is backed by a file, or null or empty otherwise.
        /// </param>
        /// <returns>
        /// This object<see cref="T"/>.
        /// </returns>
        protected T SetFileBackedSource(string fileBackedSource)
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
    }
}