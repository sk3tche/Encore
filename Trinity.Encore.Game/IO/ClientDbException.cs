using System;
using System.Runtime.Serialization;

namespace Trinity.Encore.Game.IO
{
    [Serializable]
    public class ClientDbException : Exception
    {
        public ClientDbException()
        {
        }

        public ClientDbException(string message)
            : base(message)
        {
        }

        public ClientDbException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ClientDbException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
