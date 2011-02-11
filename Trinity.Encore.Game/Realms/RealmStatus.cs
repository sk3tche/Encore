using System;

namespace Trinity.Encore.Game.Realms
{
    [Serializable]
    public enum RealmStatus : byte
    {
        Open = 0,
        Locked = 1,
    }
}
