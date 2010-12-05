using System.Diagnostics.Contracts;

namespace Trinity.Encore.Framework.Game.IO.Formats.DBC
{
    public sealed class SpellEffectRecord : IClientDbRecord
    {
        public int Id { get; set; }
        public int Effect { get; set; }
        public float Amplitude { get; set; }
        public int Aura { get; set; }
        public int AuraPeriod { get; set; }
        public int BasePoints { get; set; }
        public float Unk { get; set; }
        public float ChainAmplitude { get; set; }
        public int ChainTargets { get; set; }
        public int DieSides { get; set; }
        public int ItemType { get; set; }
        public int Mechanic { get; set; }
        public int MiscValue { get; set; }
        public int MiscValueB { get; set; }
        public int PointsPerCombo { get; set; }
        public int RadiusIndex { get; set; }
        public int RadiusMaxIndex { get; set; }
        public float RealPointsPerLevel { get; set; }
        public SpellClassMaskData SpellClassMask { get; set; }
        public int TriggerSpell { get; set; }
        public int ImplicitTargetA { get; set; }
        public int ImplicitTargetB { get; set; }
        public int SpellID { get; set; }
        public int EffectIndex { get; set; }
    }

    public sealed class SpellClassMaskData
    {
        private int[] _data = new int[3];
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
