using System;

namespace Trinity.Encore.Game.Achievements
{
    [Serializable]
    [Flags]
    public enum AchievementFlags : uint
    {
        /// <summary>
        /// Just count statistic (never stop/complete).
        /// </summary>
        IsCounter = 0x00000001,
        /// <summary>
        /// Not sent to client - for internal use only.
        /// </summary>
        InternalTracking = 0x00000002,
        /// <summary>
        /// Store only max criteria value? Used only in "reach level" achievements.
        /// </summary>
        StoreMaxCriteriaValue = 0x00000004,
        /// <summary>
        /// Use sum of criteria values from all requirements (and calculate max value).
        /// </summary>
        UseRequirementSum = 0x00000008,
        /// <summary>
        /// Show max criteria (and calculate max value?).
        /// </summary>
        ShowMaxCriteriaValue = 0x00000010,
        /// <summary>
        /// Use nonzero requirements count (and calculate max value).
        /// </summary>
        HasRequirementCount = 0x00000020,
        /// <summary>
        /// Show as average value (value / time in days), depends on other flags (by default uses last criteria value).
        /// </summary>
        ShowAverageValue = 0x00000040,
        /// <summary>
        /// Show as progress bar (value / max value), depends on other flags (by default uses last criteria value).
        /// </summary>
        ShowProgressBar = 0x00000080,
        /// <summary>
        /// One of a set of achievements gearing around being the first to reach maximum level on a given realm.
        /// </summary>
        RealmFirstReachLevel = 0x00000100,
        /// <summary>
        /// One of a set of achievements gearing around being the first to slay a given creature on a given realm.
        /// </summary>
        RealmFirstKill = 0x00000200,
    }
}
