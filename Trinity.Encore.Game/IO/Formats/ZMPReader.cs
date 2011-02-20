using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

namespace Trinity.Encore.Game.IO.Formats
{
    public sealed class ZMPReader : BinaryFileReader
    {
        public const int AxisSize = 128;

        public ZMPReader(string fileName)
            : base(fileName, Encoding.ASCII)
        {
            Contract.Requires(!string.IsNullOrEmpty(fileName));

            Data = new int[AxisSize, AxisSize];
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(Data != null);
        }

        public int[,] Data { get; private set; }

        protected override void Read(BinaryReader reader)
        {
            for (var x = 0; x < AxisSize; x++)
                for (var y = 0; y < AxisSize; y++)
                    Data[x, y] = reader.ReadInt32();
        }
    }
}
