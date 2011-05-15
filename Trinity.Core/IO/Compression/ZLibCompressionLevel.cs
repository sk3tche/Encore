using System;

namespace Trinity.Core.IO.Compression
{
    [Serializable]
    public enum ZLibCompressionLevel : sbyte
    {
        Default = -1,
        NoCompression = 0,
        Fastest = 1,
        Deflated = 8,
        Best = 9,
    }
}
