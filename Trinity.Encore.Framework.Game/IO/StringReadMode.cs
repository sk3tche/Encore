namespace Trinity.Encore.Framework.Game.IO
{
    public enum StringReadMode : byte
    {
        /// <summary>
        /// Series of null-terminated C strings.
        /// </summary>
        StringTable1,
        /// <summary>
        /// Series of null-terminated C strings, prefixed by a 16-bit length.
        /// </summary>
        StringTable2,
        /// <summary>
        /// Strings are read directly from the stream.
        /// </summary>
        Direct,
    }
}
