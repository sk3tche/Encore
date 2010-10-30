namespace Trinity.Encore.Framework.Game.Entities
{
    /// <summary>
    /// Rpresents the GUID of an object in the game world.
    /// </summary>
    public struct EntityGuid
    {
        public static readonly EntityGuid Zero = new EntityGuid(0);

        public readonly ulong Full;

        public EntityGuid(ulong full)
        {
            Full = full;
        }
    }
}
