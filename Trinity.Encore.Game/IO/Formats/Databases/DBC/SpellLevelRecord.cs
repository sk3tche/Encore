using System.Diagnostics.Contracts;

namespace Trinity.Encore.Game.IO.Formats.Databases.DBC
{
    [ContractVerification(false)]
    public sealed class SpellLevelRecord : IClientDbRecord
    {
        public int Id { get; set; }

        public int BaseLevel { get; set; }

        public int MaxLevel { get; set; }

        public int SpellLevel { get; set; }
    }
}
