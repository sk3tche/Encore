using System;

namespace Trinity.Encore.Game.Realms
{
    [Flags]
    [Serializable]
    public enum RealmFlags : byte
    {
        None = 0x00,
        Invalid = 0x01,
        Offline = 0x02,
        SpecifyBuild = 0x04,
        Unknown1 = 0x08,
        Unknown2 = 0x10,
        Recommended = 0x20,
        New = 0x40,
        Full = 0x80,
    }
}
