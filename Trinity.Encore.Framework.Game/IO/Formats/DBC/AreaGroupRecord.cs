using System.Diagnostics.Contracts;

namespace Trinity.Encore.Framework.Game.IO.Formats.DBC
{
    public sealed class AreaGroupRecord : IClientDbRecord
    {
        public int Id { get; set; }
        public AreaGroupData Group { get; set; }
        public int NextGroup { get; set; }
    }

    public sealed class AreaGroupData
    {
        private int[] _data = new int[6];
        public int this[int index]
        {
            get
            {
                Contract.Requires(index < _data.Length);
                return _data[index];
            }
            set
            {
                Contract.Requires(index < _data.Length);
                _data[index] = value;
            }

        }
    }
}
