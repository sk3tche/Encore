using System;
using System.Runtime.Serialization;

namespace Trinity.Core.Reflection
{
    /// <summary>
    /// The exception that is thrown when some functionality which relies on reflection
    /// encounters an error.
    /// </summary>
    [Serializable]
    public class ReflectionException : Exception
    {
        public ReflectionException()
        {
        }

        public ReflectionException(string message)
            : base(message)
        {
        }

        public ReflectionException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ReflectionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
