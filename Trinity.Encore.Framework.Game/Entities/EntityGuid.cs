using System;
using Trinity.Encore.Framework.Core;
using Trinity.Encore.Framework.Core.InteropServices;

namespace Trinity.Encore.Framework.Game.Entities
{
    /// <summary>
    /// Rpresents the GUID of an object in the game world.
    /// </summary>
    public struct EntityGuid : IEquatable<EntityGuid>
    {
        public static readonly EntityGuid Zero = new EntityGuid(0);

        public readonly ulong Full;

        public EntityGuid(ulong full)
        {
            Full = full;
        }

        public bool Equals(EntityGuid other)
        {
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
            return Utilities.GetHashCode(Full);
        }
    }
}
