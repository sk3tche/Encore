namespace Trinity.Encore.Framework.Game.IO.Formats.DBC
{
    public sealed class AchievementCategoryRecord : IClientDbRecord
    {
        public int Id { get; set; }

        public int ParentId { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }
    }
}
