using System.Diagnostics.Contracts;

namespace Trinity.Encore.Game.IO.Formats.Databases.DBC
{
    [ContractVerification(false)]
    public sealed class SpellEffectRecord : IClientDbRecord
    {
        public int Id { get; set; }

        public int Effect { get; set; }

        public float Amplitude { get; set; }

        public int Aura { get; set; }

        public int AuraPeriod { get; set; }

        public int BasePoints { get; set; }

        public float Unknown { get; set; }

        public float ChainAmplitude { get; set; }

        public int ChainTargets { get; set; }

        public int DieSides { get; set; }

        public int ItemType { get; set; }

        public int Mechanic { get; set; }

        public int MiscValueA { get; set; }

        public int MiscValueB { get; set; }

        public int PointsPerCombo { get; set; }

        public int RadiusIndex { get; set; }

        public int RadiusMaxIndex { get; set; }

        public float RealPointsPerLevel { get; set; }

        public SpellClassMask ClassMask { get; set; }

        public int TriggerSpell { get; set; }

        public int ImplicitTargetA { get; set; }

        public int ImplicitTargetB { get; set; }

        public int SpellId { get; set; }

        public int EffectIndex { get; set; }

        public sealed class SpellClassMask
        {
            public int Part1 { get; set; }

            public int Part2 { get; set; }

            public int Part3 { get; set; }
        }
    }
}
