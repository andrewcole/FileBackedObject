// <copyright file="JsonFileBackedObject.cs" company="Illallangi Enterprises">Copyright © 2012 Illallangi Enterprises</copyright>

using System.IO;
using System.Web.Script.Serialization;

namespace Illallangi
{
    /// <summary>
    /// An class that allows serializing to and from a JSON file.
    /// </summary>
    /// <typeparam name="T">The type to back.</typeparam>
    public class JsonFileBackedObject<T> : FileBackedObject<T> where T : JsonFileBackedObject<T>
    {
        #region Methods

        #region Non-Static Methods

        /// <summary>
        /// Serializes this T to a JSON string.
        /// </summary>
        /// <returns>A JSON string serialization of this T.</returns>
        public override string ToString()
        {
            return new JavaScriptSerializer().Serialize(this);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Deserializes the specified JSON file to a T.
        /// </summary>
        /// <param name="fileName">The file to deserialize.</param>
        /// <returns>A T deserialized from the specified JSON file.</returns>
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
            return new JavaScriptSerializer().Deserialize<T>(input);
        }

        #endregion

        #endregion
    }
}
