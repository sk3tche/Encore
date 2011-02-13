using System.Diagnostics.Contracts;

namespace Trinity.Encore.Game.IO.Formats.Databases.DBC
{
    [ContractVerification(false)]
    public sealed class AchievementCriteriaRecord : IClientDbRecord
    {
        public int Id { get; set; }

        public int ReferencedAchievementId { get; set; }

        public int RequiredType { get; set; }

        public int Criteria1 { get; set; }

        public int Criteria2 { get; set; }

        public CriteriaRequirementData Requirement1 { get; set; }

        public CriteriaRequirementData Requirement2 { get; set; }

        public string Name { get; set; }

        public int NameFlags { get; set; }

        public int CompletionFlag { get; set; }

        public int TimedType { get; set; }

        /// <summary>
        /// For timed spells, this is the spell ID, while for timed kills, it is the creature entry ID.
        /// </summary>
        public int TimerStartId { get; set; }

        public int TimeLimit { get; set; }

        public int Order { get; set; }

        public sealed class CriteriaRequirementData
        {
            public int Type { get; set; }

            public int Value { get; set; }
        }
    }
}
