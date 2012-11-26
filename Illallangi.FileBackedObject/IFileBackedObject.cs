// <copyright file="IFileBackedObject.cs" company="Illallangi Enterprises">Copyright © 2012 Illallangi Enterprises</copyright>

namespace Illallangi
{
    /// <summary>
    /// An interface that allows serializing to and from a file.
    /// </summary>
    /// <typeparam name="T">The type to back.</typeparam>
    public interface IFileBackedObject<out T> where T : IFileBackedObject<T>
    {
        /// <summary>
        /// Serializes this T to a file.
        /// </summary>
        /// <param name="fileName">The filename to serialize to.</param>
        /// <returns>The object being serialized.</returns>
        T ToFile(string fileName);

        /// <summary>
        /// Serializes this T to a string.
        /// </summary>
        /// <returns>A string serialization of this T.</returns>
        string ToString();
    }
}