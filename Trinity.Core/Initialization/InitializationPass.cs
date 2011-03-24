using System;

namespace Trinity.Core.Initialization
{
    /// <summary>
    /// Indicates which pass an initializable routine is to be executed in.
    /// </summary>
    [Serializable]
    public enum InitializationPass : byte
    {
        Framework,
        First,
        Second,
        Third,
        Fourth,
        Fifth,
        Sixth,
        Seventh,
        Eighth,
        Ninth,
        Tenth,
    }
}
