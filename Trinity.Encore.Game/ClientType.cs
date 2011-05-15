using System;

namespace Trinity.Encore.Game
{
    [Serializable]
    public enum ClientType : byte
    {
        /// <summary>
        /// Maps to WoWT. The client is a PTR (Public Test Realm) build.
        /// </summary>
        Test = 0,
        /// <summary>
        /// Maps to WoWB. The client is a beta build.
        /// </summary>
        Beta = 1,
        /// <summary>
        /// Maps to WoW. The client is a normal live build.
        /// </summary>
        Normal = 2,
        /// <summary>
        /// Maps to WoWI. The client is a normal live build, but in a streaming state (installing data).
        /// </summary>
        Installing = 3,
    }
}
