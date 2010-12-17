using System.Diagnostics.Contracts;

namespace Trinity.Encore.Framework.Game.IO.Formats.DBC
{
    [ContractVerification(false)]
    public sealed class ArmorLocationRecord : IClientDbRecord
    {
        /// <summary>
        /// Also inventory type?
        /// </summary>
        public int Id { get; set; }
        public float Value1 { get; set; }
        public float Value2 { get; set; }
        public float Value3 { get; set; }
        public float Value4 { get; set; }
        public float Value5 { get; set; }
    }
}
