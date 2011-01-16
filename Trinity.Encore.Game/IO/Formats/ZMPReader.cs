using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

namespace Trinity.Encore.Game.IO.Formats
{
    public sealed class ZMPReader : BinaryFileReader
    {
        public ZMPReader(string fileName)
            : base(fileName, Encoding.ASCII)
        {
            Contract.Requires(!string.IsNullOrEmpty(fileName));

            Data = new int[128, 128];
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(Data != null);
        }

        public int[,] Data { get; private set; }

        protected override void Read(BinaryReader reader)
        {
            for (var x = 0; x < 128; x++)
                for (var y = 0; y < 128; y++)
                    Data[x, y] = reader.ReadInt32();
        }
    }
}
