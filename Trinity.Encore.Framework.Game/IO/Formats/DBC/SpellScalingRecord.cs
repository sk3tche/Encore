using System.Diagnostics.Contracts;

namespace Trinity.Encore.Framework.Game.IO.Formats.DBC
{
    public sealed class SpellScalingRecord : IClientDbRecord
    {
        public int Id { get; set; }
        public int CastMin { get; set; } // milliseconds
        public int CastMax { get; set; } // milliseconds
        public int CastDiv { get; set; }
        public int Class { get; set; }

        // the following are not verified; however, SimCraft uses these names
        public SpellScalingData Average { get; set; }
        public SpellScalingData Delta { get; set; }
        public SpellScalingData BCP { get; set; }
        public float Scaling { get; set; }
        public float ScalingThreshold { get; set; }

        /// <summary>
        /// Gets the real cast time for this data at the given level.
        /// </summary>
        /// <param name="level">The level of the player casting a spell with this scaling data.</param>
        /// <returns>The appropriate cast time in milliseconds for his level.</returns>
        public int GetCastTimeForLevel(int level)
        {
            return (CastMin + ((CastMax - CastMin) / (CastDiv - 1)) * (level - 1));
        }
    }

    public sealed class SpellScalingData
    {
        private float[] _data = new float[3];
        public float this[int index]
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
