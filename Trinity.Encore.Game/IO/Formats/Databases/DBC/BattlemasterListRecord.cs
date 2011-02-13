using System.Diagnostics.Contracts;

namespace Trinity.Encore.Game.IO.Formats.Databases.DBC
{
    [ContractVerification(false)]
    public sealed class BattlemasterListRecord : IClientDbRecord
    {
        public int Id { get; set; }

        public int Map1 { get; set; }

        public int Map2 { get; set; }

        public int Map3 { get; set; }

        public int Map4 { get; set; }

        public int Map5 { get; set; }

        public int Map6 { get; set; }

        public int Map7 { get; set; }

        public int Map8 { get; set; }

        public int Type { get; set; }

        [RealType(typeof(int))]
        public bool CanJoinAsGroup { get; set; }

        public string Name { get; set; }

        public int MaxGroupSize { get; set; }

        public int HolidayWorldStateId { get; set; }

        public int MinLevel { get; set; }

        public int MaxLevel { get; set; }

        public int MaxGroupSizeRated { get; set; }

        public int MaxPlayers { get; set; }

        public int Rated { get; set; }
    }
}
