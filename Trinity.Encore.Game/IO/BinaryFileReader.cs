using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

namespace Trinity.Encore.Game.IO
{
    [ContractClass(typeof(BinaryFileReaderContracts))]
    public abstract class BinaryFileReader
    {
        protected BinaryFileReader(string fileName, Encoding encoding)
        {
            Contract.Requires(!string.IsNullOrEmpty(fileName));
            Contract.Requires(encoding != null);

            FileName = fileName;
            Encoding = encoding;
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(!string.IsNullOrEmpty(FileName));
        }

        public string FileName { get; private set; }

        public Encoding Encoding { get; private set; }

        protected void ReadFile()
        {
            var stream = File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            using (var reader = new BinaryReader(stream, Encoding))
                Read(reader);
        }

        protected abstract void Read(BinaryReader reader);
    }

    [ContractClassFor(typeof(BinaryFileReader))]
    public abstract class BinaryFileReaderContracts : BinaryFileReader
    {
        protected BinaryFileReaderContracts(string fileName, Encoding encoding)
            : base(fileName, encoding)
        {
        }

        protected override void Read(BinaryReader reader)
        {
            Contract.Requires(reader != null);
        }
    }
}
