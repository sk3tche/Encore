using System.Diagnostics.Contracts;

namespace Trinity.Encore.Framework.Game.IO.Formats.DBC
{
    [ContractVerification(false)]
    public sealed class AreaGroupRecord : IClientDbRecord
    {
        public int Id { get; set; }

        public int AreaId1 { get; set; }

        public int AreaId2 { get; set; }

        public int AreaId3 { get; set; }

        public int AreaId4 { get; set; }

        public int AreaId5 { get; set; }

        public int AreaId6 { get; set; }

        public int NextGroupId { get; set; }
    }
}
