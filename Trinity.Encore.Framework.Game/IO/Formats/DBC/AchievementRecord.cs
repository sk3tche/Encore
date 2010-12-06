using System;
using System.Diagnostics.Contracts;
using Trinity.Encore.Framework.Game.Achievements;

namespace Trinity.Encore.Framework.Game.IO.Formats.DBC
{
    [ContractVerification(false)]
    public sealed class AchievementRecord : IClientDbRecord
    {
        public int Id { get; set; }

        public int FactionId { get; set; }

        public int MapId { get; set; }

        public int ParentId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }

        public int Points { get; set; }

        public int Order { get; set; }

        public AchievementFlags Flags { get; set; }

        public int Icon { get; set; }

        public string Title { get; set; }

        public int Count { get; set; }

        public int ReferencedAchievementId { get; set; }
    }
}
