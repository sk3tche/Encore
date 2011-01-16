using System.Diagnostics.Contracts;

namespace Trinity.Encore.Game.IO.Formats.DBC
{
    [ContractVerification(false)]
    public sealed class AreaPOIRecord : IClientDbRecord
    {
        public int Id { get; set; }

        public int Icon1 { get; set; }

        public int Icon2 { get; set; }

        public int Icon3 { get; set; }

        public int Icon4 { get; set; }

        public int Icon5 { get; set; }

        public int Icon6 { get; set; }

        public int Icon7 { get; set; }

        public int Icon8 { get; set; }

        public int Icon9 { get; set; }

        public int Icon10 { get; set; }

        public int Icon11 { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public int Map { get; set; }

        public int Unknown1 { get; set; }

        public int Zone { get; set; }

        public string Name1 { get; set; }

        public string Name2 { get; set; }

        public int WorldState { get; set; }

        public float Unknown2 { get; set; }

        public int Unknown3 { get; set; }
    }
}
