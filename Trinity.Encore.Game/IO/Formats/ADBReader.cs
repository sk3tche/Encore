using System.Diagnostics.Contracts;

namespace Trinity.Encore.Game.IO.Formats
{
    public sealed class ADBReader<T> : DB2Reader<T>
        where T : class, IClientDbRecord, new()
    {
        /// <summary>
        /// ADB file header magic number (WCH2).
        /// </summary>
        public new const int HeaderMagicNumber = 0x32484357;

        public ADBReader(string fileName)
            : base(fileName)
        {
            Contract.Requires(!string.IsNullOrEmpty(fileName));
        }

        public override int? HeaderMagic
        {
            get { return HeaderMagicNumber; }
        }

        public override StringReadMode StringReadMode
        {
            get { return StringReadMode.StringTable2; }
        }
    }
}
