using System;

namespace Trinity.Encore.Framework.Game.IO.Formats.DBC
{
    public enum AchievementFaction : int
    {
        Horde = 0,
        Alliance = 1,
        Any = -1
    }

    [Flags]
    public enum AchievementFlags
    {
        /// <summary>
        /// Just count statistic (never stop and complete).
        /// </summary>
        ACHIEVEMENT_FLAG_COUNTER = 0x00000001,
        /// <summary>
        /// Not sent to client - for internal use only.
        /// </summary>
        ACHIEVEMENT_FLAG_TRACKING = 0x00000002,
        /// <summary>
        /// Store only max value? used only in "Reach level xx".
        /// </summary>
        ACHIEVEMENT_FLAG_STORE_MAX_VALUE = 0x00000004,
        /// <summary>
        /// Use sum of criteria values from all requirements (and calculate max value).
        /// </summary>
        ACHIEVEMENT_FLAG_SUMM = 0x00000008,
        /// <summary>
        /// Show max criteria (and calculate max value ??).
        /// </summary>
        ACHIEVEMENT_FLAG_MAX_USED = 0x00000010,
        /// <summary>
        /// Use nonzero requirements count (and calculate max value).
        /// </summary>
        ACHIEVEMENT_FLAG_REQ_COUNT = 0x00000020,
        /// <summary>
        /// Show as average value (value / time_in_days), depends on other flags (by default use last criteria value).
        /// </summary>
        ACHIEVEMENT_FLAG_AVERAGE = 0x00000040,
        /// <summary>
        /// Show as progress bar (value / max value), depends on other flags (by default use last criteria value).
        /// </summary>
        ACHIEVEMENT_FLAG_BAR = 0x00000080,
        /// <summary>
        /// One of a set of achievements gearing around being the first to reach maximum level on a given realm.
        /// </summary>
        ACHIEVEMENT_FLAG_REALM_FIRST_REACH = 0x00000100,
        /// <summary>
        /// One of a set of achievements gearing around being the first to slay a given creature on a given realm.
        /// </summary>
        ACHIEVEMENT_FLAG_REALM_FIRST_KILL = 0x00000200,
    };

    public sealed class AchievementRecord : IClientDbRecord
    {
        public int Id { get; set; }
        public int Faction { get; set; }
        public int Map { get; set; }
        public int Parent { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Category { get; set; }
        public int Points { get; set; }
        public int Order { get; set; }
        public AchievementFlags Flags { get; set; }
        public int Icon { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
        public int ReferencedAchievement { get; set; }
    }
}
