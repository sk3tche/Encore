using System.Diagnostics.Contracts;

namespace Trinity.Encore.Game.IO.Formats.DBC
{
    [ContractVerification(false)]
    public sealed class AchievementCategoryRecord : IClientDbRecord
    {
        public int Id { get; set; }

        public int ParentId { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }
    }
}
