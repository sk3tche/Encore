using System.Diagnostics.Contracts;

namespace Trinity.Encore.Game.IO.Formats.DBC
{
    [ContractVerification(false)]
    public sealed class SpellCastTimeRecord : IClientDbRecord
    {
        public int Id { get; set; }

        public int CastTime { get; set; }

        public int CastTimePerLevel { get; set; }

        public int MinCastTime { get; set; }
    }
}
