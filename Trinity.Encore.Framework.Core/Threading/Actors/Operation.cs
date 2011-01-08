using System;

namespace Trinity.Encore.Framework.Core.Threading.Actors
{
    [Serializable]
    public enum Operation : byte
    {
        /// <summary>
        /// Stop processing, but don't dispose (the Actor can continue processing again
        /// at any point).
        /// </summary>
        None,
        /// <summary>
        /// Continue processing.
        /// </summary>
        Continue,
        /// <summary>
        /// Stop processing and dispose the Actor.
        /// </summary>
        Dispose,
    }
}
