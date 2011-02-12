using System;
using System.Runtime.Serialization;

namespace Trinity.Core.Configuration
{
    /// <summary>
    /// The exception that is thrown when a configuration value is invalid.
    /// </summary>
    [Serializable]
    public class ConfigurationValueException : Exception
    {
        public ConfigurationValueException()
        {
        }

        public ConfigurationValueException(string message)
            : base(message)
        {
        }

        public ConfigurationValueException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ConfigurationValueException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
