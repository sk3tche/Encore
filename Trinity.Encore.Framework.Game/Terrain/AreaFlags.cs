using System;

namespace Trinity.Encore.Framework.Game.Terrain
{
    [Flags]
    [Serializable]
    public enum AreaFlags : uint
    {
        /// <summary>
        /// Snow: Only Dun Morough, Naxxramas, Razorfen Downs (?) and Winterspring.
        /// </summary>
        Snow = 0x00000001,
        /// <summary>
        /// Maybe Necropolis?
        /// </summary>
        Unknown1 = 0x00000002,
        /// <summary>
        /// Only used for areas on map 571.
        /// </summary>
        Northrend = 0x00000004,
        /// <summary>
        /// City and city subzones.
        /// </summary>
        SlaveCapital1 = 0x00000008,
        /// <summary>
        /// Can't find a common meaning.
        /// </summary>
        Unknown2 = 0x00000010,
        /// <summary>
        /// Slave capital city flag?
        /// </summary>
        SlaveCapital2 = 0x00000020,
        /// <summary>
        /// Many zones have this flag.
        /// </summary>
        Unknown3 = 0x00000040,
        /// <summary>
        /// Arena - both instanced and world arenas.
        /// </summary>
        Arena = 0x00000080,
        /// <summary>
        /// Main capital city flag.
        /// </summary>
        Capital = 0x00000100,
        /// <summary>
        /// Only used for one zone named "City"
        /// </summary>
        City = 0x00000200,
        /// <summary>
        /// Outland - only Eye of the Storm doesn't have this flag.
        /// </summary>
        Outland1 = 0x00000400,
        /// <summary>
        /// Sanctuary.
        /// </summary>
        Sanctuary = 0x00000800,
        /// <summary>
        /// Only Netherwing Ledge, Socrethar's Seat, Tempest Keep, The Arcatraz, The Botanica, The Mechanar,
        /// Sorrow Wing Point, Dragonspine Ridge, Netherwing Mines, Dragonmaw Base Camp, and Dragonmaw Skyway.
        /// </summary>
        NeedsFlying = 0x00001000,
        /// <summary>
        /// Not used in 3.0.3+.
        /// </summary>
        Unused1 = 0x00002000,
        /// <summary>
        /// Outland - only the Ring of Blood doesn't have this flag.
        /// </summary>
        Outland2 = 0x00004000,
        /// <summary>
        /// PvP objective area - although Death's Door also has this flag.
        /// </summary>
        Pvp = 0x00008000,
        /// <summary>
        /// Used only by instanced arenas.
        /// </summary>
        InstancedArena = 0x00010000,
        /// <summary>
        /// Not used in 3.0.3+.
        /// </summary>
        Unused2 = 0x00020000,
        /// <summary>
        /// Used only by Amani Pass and Hatchet Hills.
        /// </summary>
        Unknown4 = 0x00040000,
        /// <summary>
        /// Valgarde and Acherus: The Ebon Hold.
        /// </summary>
        Unknown5 = 0x00080000,
        /// <summary>
        /// Used for some starting areas where the area level is below 15.
        /// </summary>
        LowLevel = 0x00100000,
        /// <summary>
        /// Small towns (with inns).
        /// </summary>
        Town = 0x00200000,
        /// <summary>
        /// Warsong Hold, Acherus: The Ebon Hold, New Agamand Inn, and Vengeance Landing Inn.
        /// </summary>
        Unknown6 = 0x00400000,
        /// <summary>
        /// Westguard Inn, Acherus: The Ebon Hold, and Valgarde.
        /// </summary>
        Unknown7 = 0x00800000,
        /// <summary>
        /// Wintergrasp and subzones.
        /// </summary>
        OutdoorPvp1 = 0x01000000,
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
        OutdoorPvp2 = 0x08000000,
        /// <summary>
        /// Marks zones where you cannot fly.
        /// </summary>
        NoFlying = 0x20000000,
    }
}
