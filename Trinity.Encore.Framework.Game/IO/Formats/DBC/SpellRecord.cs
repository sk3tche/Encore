using System;
using System.Diagnostics.Contracts;

namespace Trinity.Encore.Framework.Game.IO.Formats.DBC
{
    public sealed class SpellRecord : IClientDbRecord
    {
        public int Id { get; set; }
        public SpellAttributes Attributes { get; set; }
        public int Unk { get; set; }
        public int CastingTimeIndex { get; set; }
        public int DurationIndex { get; set; }
        public int PowerType { get; set; }
        public int RangeIndex { get; set; }
        public int Speed { get; set; }
        public int SpellVisualID1 { get; set; }
        public int SpellVisualID2 { get; set; }
        public int SpellIconID { get; set; }
        public int ActiveIconID { get; set; }
        public string Name { get; set; } // unused
        public string NameSubtext { get; set; }
        public string Description { get; set; }
        public string AuraDescription { get; set; }
        public int SchoolMask { get; set; }
        public int RuneCostID { get; set; }
        public int MissileID { get; set; }
        public int DescriptionFlags { get; set; }
        public int DifficultyID { get; set; }
        public float UnkFloat { get; set; }
        public int ScalingID { get; set; }
        public int AuraOptionsID { get; set; }
        public int AuraRestrictionsID { get; set; }
        public int CastingRequirementsID { get; set; }
        public int CategoryID { get; set; }
        public int ClassOptionsID { get; set; }
        public int CooldoownID { get; set; }
        public int Unk2 { get; set; }
        public int EquippedItemsID { get; set; }
        public int InterruptsID { get; set; }
        public int LevelID { get; set; }
        public int PowerID { get; set; }
        public int ReagentsID { get; set; }
        public int ShapeshiftID { get; set; }
        public int TargetRestrictionsID { get; set; }
        public int TotemsID { get; set; }
        public int Unk3 { get; set; }

        public SpellScalingRecord GetAssociatedSpellScalingRecord()
        {
            throw new NotImplementedException();
        }

        public SpellLevelRecord GetAssociatedSpellLevelRecord()
        {
            throw new NotImplementedException();
        }

        public SpellCastTimeRecord GetAssociatedSpellCastTimeRecord()
        {
            throw new NotImplementedException();
        }
    }

    public sealed class SpellAttributes
    {
        private int[] _data = new int[9];
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
