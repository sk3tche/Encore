using System.Diagnostics.Contracts;

namespace Trinity.Encore.Game.IO.Formats.Databases.DBC
{
    [ContractVerification(false)]
    public sealed class BankBagSlotPricesRecord : IClientDbRecord
    {
        public int Id { get; set; }

        public int Price { get; set; }
    }
}
