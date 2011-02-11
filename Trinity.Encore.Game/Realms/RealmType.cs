using System;

namespace Trinity.Encore.Game.Realms
{
    [Serializable]
    public enum RealmType : byte
    {
        Normal = 0,
        Pvp = 1,
        Unknown1 = 2,
        Unknown2 = 3,
        Unknown3 = 4,
        Unknown4 = 5,
        RP = 6,
        RpPvp = 7,
    }
}
