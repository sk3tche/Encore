using System.Diagnostics.Contracts;

namespace Trinity.Encore.Game.IO.Formats.DBC
{
    [ContractVerification(false)]
    public sealed class GTSpellScalingRecord : IClientDbRecord
    {
        public int Id { get; set; }

        public float Coefficient { get; set; }
    }
}
