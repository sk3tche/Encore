using System;
using Trinity.Encore.Framework.Core;
using Trinity.Encore.Framework.Core.Runtime;

namespace Trinity.Encore.Framework.Game.Entities
{
    /// <summary>
    /// Represents the spawn ID of an object in the game world.
    /// 
    /// The ID may not always be world-unique, as some types of GUIDs are map-local.
    /// </summary>
    [Serializable]
    public struct EntityGuid : IEquatable<EntityGuid>
    {
        public static readonly EntityGuid Zero = new EntityGuid(0);

        [CLSCompliant(false)]
        public readonly ulong Full;

        [CLSCompliant(false)]
        public EntityGuid(ulong full)
        {
            Full = full;
        }

        public bool Equals(EntityGuid other)
        {
            return Full == other.Full;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is EntityGuid))
                return false;

            return Equals((EntityGuid)obj);
        }

        public static bool operator ==(EntityGuid a, EntityGuid b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(EntityGuid a, EntityGuid b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return HashCodeUtility.GetHashCode(Full);
        }
    }
}
