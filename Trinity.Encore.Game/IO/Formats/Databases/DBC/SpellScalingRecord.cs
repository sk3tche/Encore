using System.Diagnostics.Contracts;

namespace Trinity.Encore.Game.IO.Formats.Databases.DBC
{
    [ContractVerification(false)]
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
        /// <returns>The appropriate cast time in milliseconds for the given level.</returns>
        public int GetCastTimeForLevel(int level)
        {
            Contract.Requires(level > 0);
            Contract.Requires(level < Constants.Game.MaxLevelCap);

            var castTime = (CastMin + ((CastMax - CastMin) / (CastDiv - 1)) * (level - 1));
            if (castTime > CastMax)
                castTime = CastMax;

            return castTime;
        }

        public sealed class SpellScalingData
        {
            public float Part1 { get; set; }

            public float Part2 { get; set; }

            public float Part3 { get; set; }
        }
    }
}
