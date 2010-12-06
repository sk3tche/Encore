using System;

namespace Trinity.Encore.Framework.Game.Achievements
{
    [Serializable]
    public enum AchievementFaction : uint
    {
        Horde = 0,
        Alliance = 1,
        Any = uint.MaxValue,
    }
}
