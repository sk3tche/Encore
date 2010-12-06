namespace Trinity.Encore.Framework.Game.IO.Formats.DBC
{
    public sealed class SpellCastTimeRecord : IClientDbRecord
    {
        public int Id { get; set; }

        public int CastTime { get; set; }

        public int CastTimePerLevel { get; set; }

        public int MinCastTime { get; set; }
    }
}
