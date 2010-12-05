using System;

namespace Trinity.Encore.Framework.Game.IO.Formats.DBC
{
    [Flags]
    public enum AreaFlags : int
    {
        /// <summary>
        /// Snow: Only Dun Morough, Naxxramas, Razorfen Downs (?) and Winterspring.
        /// </summary>
        Snow = 0x00000001,
        /// <summary>
        /// Maybe Necropolis?
        /// </summary>
        Unk1 = 0x00000002,
        /// <summary>
        /// Only used for areas on map 571.
        /// </summary>
        Unk2 = 0x00000004,
        /// <summary>
        /// City and city subzones.
        /// </summary>
        SlaveCapital = 0x00000008,
        /// <summary>
        /// Can't find a common meaning.
        /// </summary>
        Unk3 = 0x00000010,
        /// <summary>
        /// Slave capital city flag?
        /// </summary>
        SlaveCapital2 = 0x00000020,
        /// <summary>
        /// Many zones have this flag.
        /// </summary>
        Unk4 = 0x00000040,
        /// <summary>
        /// Arena - both instanced and world arenas.
        /// </summary>
        Arena = 0x00000080,
        /// <summary>
        /// Main capital city flag
        /// </summary>
        Capital = 0x00000100,
        /// <summary>
        /// Only used for one zone named "City"
        /// </summary>
        City = 0x00000200,
        /// <summary>
        /// Outland - only Eye of the Storm doesn't have this flag.
        /// </summary>
        Outland = 0x00000400,
        /// <summary>
        /// Sanctuary.
        /// </summary>
        Sanctuary = 0x00000800,
        /// <summary>
        /// Only Netherwing Ledge, Socrethar's Seat, Tempest Keep, The Arcatraz, The Botanica, The Mechanar, Sorrow Wing Point, Dragonspine Ridge, Netherwing Mines, Dragonmaw Base Camp, Dragonmaw Skyway
        /// </summary>
        NeedFly = 0x00001000,
        /// <summary>
        /// Not used in 3.0.3
        /// </summary>
        Unused1 = 0x00002000,
        /// <summary>
        /// Outland - only the Ring of Blood doesn't have this flag.
        /// </summary>
        Outland2 = 0x00004000,
        /// <summary>
        /// PVP objective area - although Death's Door also has this flag.
        /// </summary>
        PVP = 0x00008000,
        /// <summary>
        /// Used only by instanced arenas.
        /// </summary>
        InstancedArena = 0x00010000,
        /// <summary>
        /// Not used in 3.0.3
        /// </summary>
        Unused2 = 0x00020000,
        /// <summary>
        /// Used only by Amani Pass and Hatchet Hills
        /// </summary>
        Unk5 = 0x00040000,
        /// <summary>
        /// Valgarde and Acherus: The Ebon Hold
        /// </summary>
        Unk6 = 0x00080000,
        /// <summary>
        /// Used for some starting areas where the area level is below 15.
        /// </summary>
        LowLevel = 0x00100000,
        /// <summary>
        /// Small towns (with inns).
        /// </summary>
        Town = 0x00200000,
        /// <summary>
        /// Warsong Hold, Acherus: The Ebon Hold, New Agamand Inn, Vengeance Landing Inn
        /// </summary>
        Unk7 = 0x00400000,
        /// <summary>
        /// Westguard Inn, Acherus: The Ebon Hold, Valgarde
        /// </summary>
        Unk8 = 0x00800000,
        /// <summary>
        /// Wintergrasp and subzones.
        /// </summary>
        OutdoorPvP = 0x01000000,
        /// <summary>
        /// Used to determine whether or not certain spells (such as mounts) can be used.
        /// </summary>
        Inside = 0x02000000,
        /// <summary>
        /// Used to determine whether or not certain spells (such as mounts) can be used.
        /// </summary>
        Outside = 0x04000000,
        /// <summary>
        /// Wintergrasp and subzones.
        /// </summary>
        OutdoorPvP2 = 0x08000000,
        /// <summary>
        /// Marks zones where you cannot fly.
        /// </summary>
        NoFlyZone = 0x20000000
    }

    public sealed class AreaTableRecord : IClientDbRecord
    {
        public int Id { get; set; }
        public int Map { get; set; }
        public int Zone { get; set; }
        public int ExploreFlag { get; set; }
        public AreaFlags Flags { get; set; }
        public int Unk5 { get; set; }
        public int Unk6 { get; set; }
        public int Unk7 { get; set; }
        public int Unk8 { get; set; }
        public int Unk9 { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public int Team { get; set; }
        public int Unk13 { get; set; }
        public int Unk14 { get; set; }
        public int Unk15 { get; set; }
        public int Unk16 { get; set; }
        public int Unk17 { get; set; }
        public int Unk18 { get; set; }
        public int Unk19 { get; set; }
        public int Unk20 { get; set; }
        public int Unk21 { get; set; }
        public int Unk22 { get; set; }
        public int Unk23 { get; set; }
        public int Unk24 { get; set; }

        public bool IsSanctuary()
        {
            // why Map == 609?
            return Map == 609 || Flags.HasFlag(AreaFlags.Sanctuary);
        }
    }
}
