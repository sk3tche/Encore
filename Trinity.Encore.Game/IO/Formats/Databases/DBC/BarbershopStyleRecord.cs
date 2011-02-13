using System.Diagnostics.Contracts;

namespace Trinity.Encore.Game.IO.Formats.Databases.DBC
{
    [ContractVerification(false)]
    public sealed class BarbershopStyleRecord : IClientDbRecord
    {
        public int Id { get; set; }

        public int Type { get; set; }

        public string Name { get; set; }

        public string Unknown { get; set; }

        public float CostMultiplier { get; set; }

        public int Race { get; set; }

        public int Gender { get; set; }

        public int Hair { get; set; }
    }
}
