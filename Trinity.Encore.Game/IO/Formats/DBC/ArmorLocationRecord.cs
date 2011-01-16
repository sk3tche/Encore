using System.Diagnostics.Contracts;

namespace Trinity.Encore.Game.IO.Formats.DBC
{
    [ContractVerification(false)]
    public sealed class ArmorLocationRecord : IClientDbRecord
    {
        // Also inventory type?
        public int Id { get; set; }

        public float Value1 { get; set; }

        public float Value2 { get; set; }

        public float Value3 { get; set; }

        public float Value4 { get; set; }

        public float Value5 { get; set; }
    }
}
