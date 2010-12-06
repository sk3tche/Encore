using System;
using System.Diagnostics.Contracts;
using Trinity.Encore.Framework.Game.Terrain;

namespace Trinity.Encore.Framework.Game.IO.Formats.DBC
{
    [ContractVerification(false)]
    public sealed class AreaTableRecord : IClientDbRecord
    {
        public int Id { get; set; }

        public int MapId { get; set; }

        public int ZoneId { get; set; }

        public int ExplorationFlag { get; set; }

        public AreaFlags Flags { get; set; }

        public int Unknown1 { get; set; }

        public int Unknown2 { get; set; }

        public int Unknown3 { get; set; }

        public int Unknown4 { get; set; }

        public int Unknown5 { get; set; }

        public int Level { get; set; }

        public string Name { get; set; }

        public int Team { get; set; }

        public int Unknown6 { get; set; }

        public int Unknown7 { get; set; }

        public int Unknown8 { get; set; }

        public int Unknown9 { get; set; }

        public int Unknown10 { get; set; }

        public int Unknown11 { get; set; }

        public int Unknown12 { get; set; }

        public int Unknown13 { get; set; }

        public int Unknown14 { get; set; }

        public int Unknown15 { get; set; }

        public int Unknown16 { get; set; }

        public int Unknown17 { get; set; }

        [SkipProperty]
        public bool IsSanctuary
        {
            get
            {
                // Map 609 is Ebon Hold. Does not have the sanctuary flag, for some reason.
                return MapId == 609 || Flags.HasFlag(AreaFlags.Sanctuary);
            }
        }
    }
}
