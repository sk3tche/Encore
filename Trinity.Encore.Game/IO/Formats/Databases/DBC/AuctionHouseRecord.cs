using System.Diagnostics.Contracts;

namespace Trinity.Encore.Game.IO.Formats.Databases.DBC
{
    [ContractVerification(false)]
    public sealed class AuctionHouseRecord : IClientDbRecord
    {
        public int Id { get; set; }

        public int Faction { get; set; }

        public int DepositPercent { get; set; }

        public int CutPercent { get; set; }

        public string Name { get; set; }
    }
}
