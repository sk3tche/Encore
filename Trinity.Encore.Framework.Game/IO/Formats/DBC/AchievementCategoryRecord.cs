namespace Trinity.Encore.Framework.Game.IO.Formats.DBC
{
    public sealed class AchievementCategoryRecord : IClientDbRecord
    {
        public int Id { get; set; }
        public int Parent { get; set; }
        public string Name { get; set; }
        public int Sort { get; set; }
    }
}
