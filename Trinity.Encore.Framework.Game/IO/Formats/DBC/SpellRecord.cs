using System;
using System.Diagnostics.Contracts;

namespace Trinity.Encore.Framework.Game.IO.Formats.DBC
{
    public sealed class SpellRecord : IClientDbRecord
    {
        public int Id { get; set; }

        public SpellAttributes Attributes { get; set; }

        public int Unknown1 { get; set; }

        public int CastingTimeIndex { get; set; }

        public int DurationIndex { get; set; }

        public int PowerType { get; set; }

        public int RangeIndex { get; set; }

        public int Speed { get; set; }

        public int SpellVisualId1 { get; set; }

        public int SpellVisualId2 { get; set; }

        public int SpellIconId { get; set; }

        public int ActiveIconId { get; set; }

        public string Name { get; set; }

        public string NameSubtext { get; set; }

        public string Description { get; set; }

        public string AuraDescription { get; set; }

        public int SchoolMask { get; set; }

        public int RuneCostId { get; set; }

        public int MissileId { get; set; }

        public int DescriptionFlags { get; set; }

        public int DifficultyId { get; set; }

        public float Unknown2 { get; set; }

        public int ScalingId { get; set; }

        public int AuraOptionsId { get; set; }

        public int AuraRestrictionsId { get; set; }

        public int CastingRequirementsId { get; set; }

        public int CategoryId { get; set; }

        public int ClassOptionsId { get; set; }

        public int CooldoownId { get; set; }

        public int Unknown3 { get; set; }

        public int EquippedItemsId { get; set; }

        public int InterruptsId { get; set; }

        public int LevelId { get; set; }

        public int PowerId { get; set; }

        public int ReagentsId { get; set; }

        public int ShapeshiftId { get; set; }

        public int TargetRestrictionsId { get; set; }

        public int TotemsId { get; set; }

        public int Unknown4 { get; set; }

        public sealed class SpellAttributes
        {
            public int Attributes1 { get; set; }

            public int Attributes2 { get; set; }

            public int Attributes3 { get; set; }

            public int Attributes4 { get; set; }

            public int Attributes5 { get; set; }

            public int Attributes6 { get; set; }

            public int Attributes7 { get; set; }

            public int Attributes8 { get; set; }

            public int Attributes9 { get; set; }
        }
    }
}
