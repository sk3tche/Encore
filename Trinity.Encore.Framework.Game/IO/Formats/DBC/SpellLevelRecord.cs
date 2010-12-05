namespace Trinity.Encore.Framework.Game.IO.Formats.DBC
{
    public sealed class SpellLevelRecord : IClientDbRecord
    {
        public int Id { get; set; }
        public int BaseLevel { get; set; }
        public int MaxLevel { get; set; }
        public int SpellLevel { get; set; }
    }
}
