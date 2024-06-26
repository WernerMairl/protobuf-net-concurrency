﻿
using System;

namespace ProtoBuf
{
    /// <summary>
    /// Indicates that the implementing type has support for protocol-buffer
    /// <see cref="IExtension">extensions</see>.
    /// </summary>
    /// <remarks>Can be implemented by deriving from Extensible.</remarks>
    public interface IExtensible
    {
        /// <summary>
        /// Retrieves the <see cref="IExtension">extension</see> object for the current
        /// instance, optionally creating it if it does not already exist.
        /// </summary>
        /// <param name="createIfMissing">Should a new extension object be
        /// created if it does not already exist?</param>
        /// <returns>The extension object if it exists (or was created), or null
        /// if the extension object does not exist or is not available.</returns>
        /// <remarks>The <c>createIfMissing</c> argument is false during serialization,
        /// and true during deserialization upon encountering unexpected fields.</remarks>
        IExtension GetExtensionObject(bool createIfMissing);
    }

    /// <summary>
    /// Indicates that the implementing type has support for protocol-buffer
    /// <see cref="IExtension">extensions</see> at multiple inheritance levels.
    /// </summary>
    /// <remarks>Can be implemented by deriving from Extensible.</remarks>
    public interface ITypedExtensible
    {
        /// <summary>
        /// Retrieves the <see cref="IExtension">extension</see> object for the current
        /// instance, optionally creating it if it does not already exist.
        /// </summary>
        /// <param name="createIfMissing">Should a new extension object be
        /// created if it does not already exist?</param>
        /// <param name="type">The <see cref="Type"/> that holds the fields, in terms of the inheritance model; the same <c>tag</c> key can appear against different <c>type</c> levels for the same <c>instance</c>, with different values.</param>
        /// <returns>The extension object if it exists (or was created), or null
        /// if the extension object does not exist or is not available.</returns>
        /// <remarks>The <c>createIfMissing</c> argument is false during serialization,
        /// and true during deserialization upon encountering unexpected fields.</remarks>
        IExtension GetExtensionObject(Type type, bool createIfMissing);
    }
}