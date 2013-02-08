// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CachingException.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the CachingException type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.BclExtensions.Caching
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// CachingException Class.
    /// </summary>
    [Serializable]
    public class CachingException : BclExtensionsException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CachingException"/> class.
        /// </summary>
        public CachingException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CachingException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public CachingException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CachingException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner Exception.</param>
        public CachingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CachingException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization info.</param>
        /// <param name="context">The context.</param>
        protected CachingException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }
    }
}
